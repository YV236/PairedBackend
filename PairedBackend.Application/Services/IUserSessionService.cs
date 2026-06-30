using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IUserSessionService
{
    Task<Result<Guid>> CreateSessionAsync(
        Guid userId,
        string device,
        string ipAddress,
        string refreshToken,
        CancellationToken cancellationToken);

    Task<Result<List<UserSessionResponse>>> GetUserSessions(Guid userId, CancellationToken cancellationToken);

    Task<Result<Guid>> GetUserIdByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task<Result<Guid>> UpdateSessionRefreshTokenAsync(
        string oldRefreshToken,
        string newRefreshToken,
        string ipAddress,
        CancellationToken cancellationToken);

    Task<Result> RevokeSessionAsync(Guid sessionId, CancellationToken cancellationToken);

    Task<Result> RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken);
}
