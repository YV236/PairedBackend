using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Application.Features.Sessions.RevokeAllSessions;
using PairedBackend.Application.Features.Sessions.RevokeSession;
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
public class UserSessionController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ApiController
{
    /// <summary>
    /// Retrieve all active sessions for the authenticated user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserSessions(CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var command = new GetUserSessionCommand(userId);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Revokes all active sessions for the authenticated user, except for the current session.
    /// </summary>
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAllSessions(CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }
        var userSessionIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("Sid")?.Value;

        if (userSessionIdClaim == null || !Guid.TryParse(userSessionIdClaim, out var userSessionId))
        {
            return Unauthorized(new { message = "User session is not authenticated." });
        }

        var command = new RevokeAllUserSessionsCommand(userId, userSessionId);
        var result = await mediator.Send(command, cancellationToken);
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
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var command = new RevokeSessionCommand(userId, sessionId);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
