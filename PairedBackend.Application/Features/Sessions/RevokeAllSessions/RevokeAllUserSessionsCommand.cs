using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Sessions.RevokeAllSessions;

public record RevokeAllUserSessionsCommand(Guid UserId, Guid CurrentSessionId) : IRequest<Result>;