using MediatR;
using Microsoft.AspNetCore.Mvc;
using PairedBackend.Application.Features.Auth.RegisterUser;
using PairedBackend.Contracts.User;

namespace PairedBackend.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IMediator mediator) : ApiController
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
}
