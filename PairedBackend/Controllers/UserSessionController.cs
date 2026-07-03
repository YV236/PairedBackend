using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Features.Sessions.GetUserSessions;
using PairedBackend.Application.Features.Sessions.RevokeAllSessions;
using PairedBackend.Application.Features.Sessions.RevokeSession;
using System.Security.Claims;

namespace PairedBackend.API.Controllers;

[Authorize]
[Route("api/user-sessions")]
[ApiController]
public class UserSessionController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ApiController
{
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
