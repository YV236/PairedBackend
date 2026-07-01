using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.RefreshToken;

internal class RefreshTokenCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await authenticationService.RefreshTokenAsync(
            request.RefreshToken,
            request.IpAddress,
            request.Device,
            cancellationToken);
    }
}
