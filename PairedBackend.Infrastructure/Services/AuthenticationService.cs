using Microsoft.AspNetCore.Identity;
using PairedBackend.Application.Features.Auth.LoginUser;
using PairedBackend.Application.Features.Auth.RefreshToken;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Identity;

namespace PairedBackend.Infrastructure.Services;

internal class AuthenticationService(UserManager<ApplicationUser> userManager, ITokenProvider tokenProvider, IUserSessionService sessionService) : IAuthenticationService
{
    public async Task<Result<Guid>> RegisterAsync(
        string email,
        string username,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = username,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await userManager.CreateAsync(user, password);

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

    // TODO: Consider adding a method to validate is session is valid and not expired or not revoked. This can be used to check if a user is still logged in or not to not create a new session.
    public async Task<Result<LoginResponse>> LoginAsync(
        string userName,
        string email,
        string password,
        string device,
        string ipAddress,
        CancellationToken cancellationToken)
    {
        ApplicationUser? user = null;

        if (!string.IsNullOrWhiteSpace(email))
        {
            user = await userManager.FindByEmailAsync(email);
        }

        if (user is null && !string.IsNullOrWhiteSpace(userName))
        {
            user = await userManager.FindByNameAsync(userName);
        }

        if (user is null)
        {
            return Result.Failure<LoginResponse>(
                new Error("Auth.InvalidCredentials", "Invalid credentials", ErrorType.Unauthorized));
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
        {
            return Result.Failure<LoginResponse>(
                new Error("Auth.InvalidCredentials", "Invalid credentials", ErrorType.Unauthorized));
        }

        var sessionId = Guid.NewGuid();

        var refreshToken = tokenProvider.GenerateRefreshToken();

        var accessToken = tokenProvider.GenerateAccessToken(user.Id, user.Email!, sessionId);

        var sessionResult = await sessionService.CreateSessionAsync(user.Id, sessionId, device, ipAddress, refreshToken, cancellationToken);

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
