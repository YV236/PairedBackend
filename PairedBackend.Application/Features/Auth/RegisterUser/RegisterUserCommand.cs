using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.RegisterUser;

public record RegisterUserCommand : IRequest<Result<Guid>>
{
    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
