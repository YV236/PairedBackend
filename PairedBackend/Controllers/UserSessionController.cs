using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Services;
using System.Security.Claims;

namespace PairedBackend.API.Controllers;

/// <summary>
/// User session is a combination of a user and a device.
/// Each device has its own session, which can be revoked separately.
/// This allows users to log out from specific devices without affecting other sessions.
/// </summary>
[Authorize]
[Route("api/user-sessions")]
[ApiController]
public class UserSessionController(IUserSessionService userSessionService) : ApiController
{
    /// <summary>
    /// Retrieve all active sessions for the authenticated user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserSessions(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var result = await userSessionService.GetUserSessions(userId, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Revokes all active sessions for the authenticated user, except for the current session.
    /// </summary>
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAllSessions(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        if (!Guid.TryParse(User.FindFirstValue("Sid"), out var sessionId))
        {
            return Unauthorized(new { message = "User session is not authenticated." });
        }

        var result = await userSessionService.RevokeAllUserSessionsAsync(userId, sessionId, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Revokes a specific session for the authenticated user by its session ID.
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    [HttpPost]
    [Route("revoke/{sessionId}")]
    public async Task<IActionResult> RevokeSessionById(Guid sessionId, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var result = await userSessionService.RevokeSessionAsync(userId, sessionId, cancellationToken);
        return HandleResult(result);
    }

    private bool TryGetUserId(out Guid userId)
    {
        return Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }
}
