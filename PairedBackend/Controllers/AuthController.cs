using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Features.Auth.LoginUser;
using PairedBackend.Application.Features.Auth.RefreshToken;
using PairedBackend.Application.Features.Auth.RegisterUser;
using PairedBackend.Contracts.User;

namespace PairedBackend.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.UserName,
            request.Password,
            request.FirstName,
            request.LastName,
            request.PhoneNumber);

        var result = await mediator.Send(command);

        return HandleResult(result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var command = new LoginUserCommand(request.UserName, request.Email, request.Password, request.Device, request.IpAddress);
        var result = await mediator.Send(command);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, cookieOptions);

        return Ok(new { AccessToken = result.Value.AccessToken });
    }

    [Authorize]
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token is missing." });
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        var userAgent = Request.Headers.UserAgent.ToString();

        var command = new RefreshTokenCommand(refreshToken, ipAddress, userAgent);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, cookieOptions);

        return Ok(new { AccessToken = result.Value.AccessToken });
    }

}
