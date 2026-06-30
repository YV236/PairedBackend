using MediatR;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Features.Auth.LoginUser;
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
        return HandleResult(result);
    }
}
