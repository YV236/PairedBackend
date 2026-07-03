using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Sessions.RevokeSession;

public record RevokeSessionCommand(Guid UserId, Guid SessionId) : IRequest<Result>;
