using PairedBackend.Application.Features.Auth.LoginUser;
using PairedBackend.Application.Features.Auth.RefreshToken;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IAuthenticationService
{
    Task<Result<Guid>> RegisterAsync(
        string email,
        string username,
        string password,
        string firstName,
        string lastName,
        string phoneNumber,
        CancellationToken cancellationToken);

    Task<Result<LoginResponse>> LoginAsync(
        string userName,
        string email,
        string password,
        string device,
        string ipAddress,
        CancellationToken cancellationToken);

    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(
        string refreshToken,
        string ipAddress,
        string device,
        CancellationToken cancellationToken);
}
