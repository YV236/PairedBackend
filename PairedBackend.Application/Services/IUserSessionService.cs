using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IUserSessionService
{
    Task<Result> CreateSessionAsync(
        Guid userId,
        Guid sessionId,
        string device,
        string ipAddress,
        string refreshToken,
        CancellationToken cancellationToken);

    Task<Result<List<UserSessionResponse>>> GetUserSessions(Guid userId, CancellationToken cancellationToken);

    Task<Result<(Guid UserId, Guid SessionId)>> GetUserIdByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task<Result> UpdateSessionRefreshTokenAsync(
        string oldRefreshToken,
        string newRefreshToken,
        string ipAddress,
        string device,
        CancellationToken cancellationToken);

    Task<Result> RevokeSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken);

    Task<Result> RevokeAllUserSessionsAsync(Guid userId, Guid currentSession, CancellationToken cancellationToken);
}
