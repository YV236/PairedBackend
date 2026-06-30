using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Infrastructure.Services;

internal class UserSessionService : IUserSessionService
{
    public Task<Result<Guid>> CreateSessionAsync(Guid userId, string device, string os, string client, string ipAddress, string location, string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Guid>> GetUserIdByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<UserSessionResponse>>> GetUserSessions(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RevokeSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateSessionRefreshTokenAsync(string oldRefreshToken, string newRefreshToken, string ipAddress, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
