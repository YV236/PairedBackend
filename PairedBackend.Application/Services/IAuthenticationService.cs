using PairedBackend.Contracts.Requests.User;
using PairedBackend.Contracts.Responses.User;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IAuthenticationService
{
    Task<Result<Guid>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);

    Task<Result<LoginResponse>> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken);

    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(
        string refreshToken,
        string ipAddress,
        string device,
        CancellationToken cancellationToken);
}
