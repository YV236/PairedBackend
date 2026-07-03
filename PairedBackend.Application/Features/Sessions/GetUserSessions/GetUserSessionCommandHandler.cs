using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Sessions.GetUserSessions;

internal class GetUserSessionCommandHandler(IUserSessionService userSessionService) : IRequestHandler<GetUserSessionCommand, Result<List<UserSessionResponse>>>
{
    public async Task<Result<List<UserSessionResponse>>> Handle(GetUserSessionCommand request, CancellationToken cancellationToken)
    {
        return await userSessionService.GetUserSessions(request.UserId, cancellationToken);
    }
}
