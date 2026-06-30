using Microsoft.AspNetCore.Identity;
using PairedBackend.Application.Features.Auth.LoginUser;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Identity;

namespace PairedBackend.Infrastructure.Services;

internal class AuthenticationService(UserManager<ApplicationUser> userManager, ITokenProvider tokenProvider, IUserSessionService sessionService) : IAuthenticationService
{
    public async Task<Result<Guid>> RegisterAsync(string email, string username, string password, string firstName, string lastName, CancellationToken cancellationToken)
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
    public Task<Result<LoginResponse>> LoginAsync(string email, string password, string device, string ipAddress, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(
    string refreshToken,
    string ipAddress,
    CancellationToken cancellationToken)
    {
        var sessionResult = await sessionService.GetUserIdByRefreshTokenAsync(refreshToken, cancellationToken);

        if (sessionResult.IsFailure)
        {
            return Result.Failure<LoginResponse>(
                new Error("Auth.InvalidRefreshToken", "Токен оновлення недійсний або прострочений", ErrorType.Unauthorized));
        }

        var userId = sessionResult.Value;

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result.Failure<LoginResponse>(
                new Error("Auth.UserNotFound", "Користувача не знайдено", ErrorType.NotFound));
        }

        var newRefreshToken = tokenProvider.GenerateRefreshToken();

        var updateResult = await sessionService.UpdateSessionRefreshTokenAsync(
            oldRefreshToken: refreshToken,
            newRefreshToken: newRefreshToken,
            ipAddress: ipAddress,
            cancellationToken: cancellationToken);

        if (updateResult.IsFailure)
        {
            return Result.Failure<LoginResponse>(updateResult.Error);
        }

        var sessionId = Guid.NewGuid();
        var newAccessToken = tokenProvider.GenerateAccessToken(user.Id, user.Email!, sessionId);

        return Result.Success(new LoginResponse(newAccessToken, newRefreshToken));
    }
}
