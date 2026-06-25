using Microsoft.AspNetCore.Mvc;
using PairedBackend.Contracts.User;

namespace PairedBackend.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IMediator mediator) : ApiController
{
    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegisterUserRequest request)
    {
        return Ok();
    }
}
