using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Services;
using PairedBackend.Contracts.User;

namespace PairedBackend.API.Controllers;

/// <summary>
/// Authenication allows user to get access to the system by providing valid credentials.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthenticationService authenticationService,
    IValidator<RegisterUserRequest> registerValidator,
    IValidator<LoginUserRequest> loginValidator) : ApiController
{
    /// <summary>
    /// Create a new user account with the provided registration details.
    /// </summary>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await registerValidator.ValidateAndThrowAsync(request, cancellationToken);
        var result = await authenticationService.RegisterAsync(
            request.Email,
            request.UserName,
            request.Password,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            cancellationToken);

        return HandleResult(result);
    }

    /// <summary>
    /// Login into the system using valid credentials and receive an access token and refresh token.
    /// </summary>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
    {
        await loginValidator.ValidateAndThrowAsync(request, cancellationToken);
        var result = await authenticationService.LoginAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.Device,
            request.IpAddress,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        AppendRefreshTokenCookie(result.Value.RefreshToken);
        return Ok(new { AccessToken = result.Value.AccessToken });
    }

    /// <summary>
    /// Refresh the access token using a valid refresh token.
    /// </summary>
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
        var result = await authenticationService.RefreshTokenAsync(refreshToken, ipAddress, userAgent, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        AppendRefreshTokenCookie(result.Value.RefreshToken);
        return Ok(new { AccessToken = result.Value.AccessToken });
    }

    private void AppendRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30)
        });
    }
}
