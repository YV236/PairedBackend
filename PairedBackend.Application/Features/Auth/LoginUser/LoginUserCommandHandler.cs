using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.LoginUser;

internal class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginUserCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await authenticationService.LoginAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.Device,
            request.IpAddress,
            cancellationToken);
    }
}