using PairedBackend.Application.Features.Auth.LoginUser;
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
        CancellationToken cancellationToken);

    Task<Result<LoginResponse>> LoginAsync(
        string email,
        string password,
        string device,
        string ipAddress,
        CancellationToken cancellationToken);

    Task<Result<LoginResponse>> RefreshTokenAsync(
        string refreshToken,
        string ipAddress,
        CancellationToken cancellationToken);
}
