using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Identity;
using PairedBackend.Infrastructure.Persistence;

namespace PairedBackend.Infrastructure.Services;

internal class UserSessionService(AppDbContext context) : IUserSessionService
{
    public async Task<Result<Guid>> CreateSessionAsync(Guid userId, Guid sessionId, string device, string ipAddress, string refreshToken, CancellationToken cancellationToken)
    {
        var session = new UserSession()
        {
            Id = sessionId,
            UserId = userId,
            Device = device,
            IPAddress = ipAddress,
            RefreshToken = refreshToken,
            CreatedAt = DateTime.UtcNow,
            LastActiveAt = DateTime.UtcNow,
            IsRevoked = false
        };

        context.UserSessions.Add(session);

        await context.SaveChangesAsync();

        return Result.Success(session.Id);
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

    public Task<Result<Guid>> UpdateSessionRefreshTokenAsync(string oldRefreshToken, string newRefreshToken, string ipAddress, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
