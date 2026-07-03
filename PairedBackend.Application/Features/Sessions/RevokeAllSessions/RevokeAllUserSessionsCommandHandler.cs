using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Sessions.RevokeAllSessions;

internal class RevokeAllUserSessionsCommandHandler(IUserSessionService userSessionService) : IRequestHandler<RevokeAllUserSessionsCommand, Result>
{
    public async Task<Result> Handle(RevokeAllUserSessionsCommand request, CancellationToken cancellationToken)
    {
        return await userSessionService.RevokeAllUserSessionsAsync(request.UserId, request.CurrentSessionId, cancellationToken);
    }
}
