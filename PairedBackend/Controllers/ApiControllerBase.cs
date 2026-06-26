using Microsoft.AspNetCore.Mvc;
using PairedBackend.Domain.Shared;

namespace PairedBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return HandleFailure(result.Error);
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return HandleFailure(result.Error);
    }

    private IActionResult HandleFailure(Error error)
    {
        var statusCode = error.ErrorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.ValidationError => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };
        return Problem(
            statusCode: statusCode,
            title: "Bad Request",
            detail: error.Message,
            extensions: new Dictionary<string, object?>
            {
                { "errorCode", error.Code }
            });
    }
}
