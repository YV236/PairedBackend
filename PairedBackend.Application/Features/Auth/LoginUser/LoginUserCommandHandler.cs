using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.LoginUser;

internal class LoginCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResponse>>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.LoginAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.Device,
            request.IpAddress,
            cancellationToken);
    }
}