using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.RegisterUser;

internal class RegisterUserCommandHandler(IAuthenticationService userService) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await userService.RegisterAsync(request.Email, request.UserName, request.Password, request.FirstName, request.LastName, cancellationToken);

        return result;
    }
}
