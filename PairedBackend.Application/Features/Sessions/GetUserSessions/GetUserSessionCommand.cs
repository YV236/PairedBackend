using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Sessions.GetUserSessions;

public record GetUserSessionCommand
(
    Guid UserId
) : IRequest<Result<List<UserSessionResponse>>>;
