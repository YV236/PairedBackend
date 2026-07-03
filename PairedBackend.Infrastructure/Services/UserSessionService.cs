using Microsoft.EntityFrameworkCore;
using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Identity;
using PairedBackend.Infrastructure.Persistence;

namespace PairedBackend.Infrastructure.Services;

internal class UserSessionService(AppDbContext context) : IUserSessionService
{
    public async Task<Result> CreateSessionAsync(
        Guid userId,
        Guid sessionId,
        string device,
        string ipAddress,
        string refreshToken,
        CancellationToken cancellationToken)
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

    public async Task<Result<(Guid UserId, Guid SessionId)>> GetUserIdByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var session = await context.UserSessions
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);

        if (session is null || session.IsRevoked)
        {
            return Result.Failure<(Guid, Guid)>(new Error("Auth.InvalidRefreshToken", "...", ErrorType.Unauthorized));
        }

        return Result.Success((session.UserId, session.Id));
    }

    public async Task<Result<List<UserSessionResponse>>> GetUserSessions(Guid userId, CancellationToken cancellationToken)
    {
        var sessions = await context.UserSessions
        .Where(s => s.UserId == userId)
        .Select(s => new UserSessionResponse(
            s.Id,
            s.Device,
            s.OS,
            s.Client,
            s.IPAddress,
            s.Location,
            s.CreatedAt,
            s.LastActiveAt
        ))
        .ToListAsync(cancellationToken);

        return Result.Success(sessions);
    }

    public Task<Result> RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RevokeSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateSessionRefreshTokenAsync(
        string oldRefreshToken,
        string newRefreshToken,
        string ipAddress,
        string device,
        CancellationToken cancellationToken)
    {
        var session = await context.UserSessions.FirstOrDefaultAsync(s => s.RefreshToken == oldRefreshToken);
        if (session is null)
        {
            return Result.Failure<Guid>(
                new Error("Auth.InvalidRefreshToken", "Token is invalid or expired", ErrorType.Unauthorized));
        }

        if (session.IsRevoked)
        {
            return Result.Failure<Guid>(
                new Error("Auth.RevokedRefreshToken", "Token is revoked", ErrorType.Unauthorized));
        }

        if (session.Device != device)
        {
            return Result.Failure<Guid>(
                new Error("Auth.InvalidDevice", "Device is invalid", ErrorType.Unauthorized));
        }

        session.RefreshToken = newRefreshToken;
        session.IPAddress = ipAddress;
        session.LastActiveAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(session.Id);
    }
}
