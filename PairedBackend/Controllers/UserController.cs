using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Services;
using PairedBackend.Contracts.Requests.User;
using System.Security.Claims;

namespace PairedBackend.API.Controllers;

/// <summary>
/// User profile data is a component of the user entity that contains personal information.
/// </summary>
[Authorize]
[Route("api/profile")]
[ApiController]
public class UserController(IUserService userService) : ApiController
{
    /// <summary>
    /// Retrives the profile data of the authenticated user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserData(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var result = await userService.GetUserProfileDataAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Changing user profile data allows the authenticated user to update their personal information.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> ChangeProfileData(ChangeProfileDataRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        var result = await userService.ChangeUserProfileData(userId, request, cancellationToken);
        return HandleResult(result);
    }

    private bool TryGetUserId(out Guid userId)
    {
        return Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }
}
