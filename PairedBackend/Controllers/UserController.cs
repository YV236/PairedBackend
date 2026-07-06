using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Features.User.ChangeData;
using PairedBackend.Application.Features.User.ProfileData;
using PairedBackend.Contracts.User;
using System.Security.Claims;

namespace PairedBackend.API.Controllers;

[Authorize]
[Route("api/profile")]
[ApiController]
public class UserController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var command = new GetUserProfileDataCommand(userId);
        var result = await mediator.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> ChangeProfileData(ChangeProfileDataRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var command = new ChangeUserProfileDataCommand(userId, request.FirstName, request.LastName, request.UserName);
        var result = await mediator.Send(command, cancellationToken);

        return HandleResult(result);
    }
}
