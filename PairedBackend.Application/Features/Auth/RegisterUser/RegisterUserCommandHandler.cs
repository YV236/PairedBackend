using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.RegisterUser;

internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
