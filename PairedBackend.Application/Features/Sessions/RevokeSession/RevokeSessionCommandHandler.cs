using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Sessions.RevokeSession;

internal class RevokeSessionCommandHandler(IUserSessionService userSessionService) : IRequestHandler<RevokeSessionCommand, Result>
{
    public async Task<Result> Handle(RevokeSessionCommand request, CancellationToken cancellationToken)
    {
        return await userSessionService.RevokeSessionAsync(request.UserId, request.SessionId, cancellationToken);
    }
}
