using Microsoft.AspNetCore.Identity;
using PairedBackend.Application.Services;
using PairedBackend.Contracts.Requests.User;
using PairedBackend.Contracts.Responses.User;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Identity;

namespace PairedBackend.Infrastructure.Services;

internal class AuthenticationService(UserManager<ApplicationUser> userManager, ITokenProvider tokenProvider, IUserSessionService sessionService) : IAuthenticationService
{
    public async Task<Result<Guid>> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return Result.Success(user.Id);
        }
        var identityError = result.Errors.FirstOrDefault();

        if (identityError is null)
        {
            return Result.Failure<Guid>(Error.Unexpected);
        }

        var error = new Error(identityError.Code, identityError.Description, ErrorType.ValidationError);

        return Result.Failure<Guid>(error);
    }

    // TODO: Consider adding a method to validate is session is valid and not expired or not revoked.
    // This can be used to check if a user is still logged in or not to not create a new session.
    public async Task<Result<LoginResponse>> LoginAsync(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        ApplicationUser? user = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            user = await userManager.FindByEmailAsync(request.Email);
        }

        if (user is null && !string.IsNullOrWhiteSpace(request.UserName))
        {
            user = await userManager.FindByNameAsync(request.UserName);
        }

        if (user is null)
        {
            return Result.Failure<LoginResponse>(
                new Error("Auth.InvalidCredentials", "Invalid credentials", ErrorType.Unauthorized));
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return Result.Failure<LoginResponse>(
                new Error("Auth.InvalidCredentials", "Invalid credentials", ErrorType.Unauthorized));
        }

        var sessionId = Guid.NewGuid();

        var refreshToken = tokenProvider.GenerateRefreshToken();

        var accessToken = tokenProvider.GenerateAccessToken(user.Id, user.Email!, sessionId);

        var sessionResult = await sessionService.CreateSessionAsync(
            user.Id,
            sessionId,
            request.Device,
            request.IpAddress,
            refreshToken,
            cancellationToken);

        if (sessionResult.IsFailure)
        {
            return Result.Failure<LoginResponse>(sessionResult.Error);
        }

        return Result.Success(new LoginResponse(accessToken, refreshToken));
    }

    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(
    string refreshToken,
    string ipAddress,
    string device,
    CancellationToken cancellationToken)
    {
        var sessionResult = await sessionService.GetUserIdByRefreshTokenAsync(refreshToken, cancellationToken);

        if (sessionResult.IsFailure)
        {
            return Result.Failure<RefreshTokenResponse>(sessionResult.Error);
        }

        var (userId, sessionId) = sessionResult.Value;

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<RefreshTokenResponse>(
                new Error("Auth.UserNotFound", "User not found", ErrorType.NotFound));
        }

        var newRefreshToken = tokenProvider.GenerateRefreshToken();

        var newAccessToken = tokenProvider.GenerateAccessToken(user.Id, user.Email!, sessionId);

        var updateSessionResult = await sessionService.UpdateSessionRefreshTokenAsync(
            refreshToken,
            newRefreshToken,
            ipAddress,
            device,
            cancellationToken);

        if (updateSessionResult.IsFailure)
        {
            return Result.Failure<RefreshTokenResponse>(updateSessionResult.Error);
        }

        return Result.Success(new RefreshTokenResponse(newAccessToken, newRefreshToken));
    }
}
