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
        .Where(s => s.UserId == userId && !s.IsRevoked)
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

    public async Task<Result> RevokeAllUserSessionsAsync(Guid userId, Guid currentSession, CancellationToken cancellationToken)
    {
        var usersessions = await context.UserSessions
            .Where(s => s.UserId == userId && s.Id != currentSession && !s.IsRevoked)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRevoked, true), cancellationToken);

        return Result.Success(usersessions);
    }

    public async Task<Result> RevokeSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken)
    {
        var rowsAffected = await context.UserSessions
            .Where(s => s.Id == sessionId && !s.IsRevoked && s.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRevoked, true), cancellationToken);

        if (rowsAffected == 0)
        {
            return Result.Failure(new Error("Auth.SessionNotFound", "Session not found or already revoked.", ErrorType.Unauthorized));
        }

        return Result.Success();
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
