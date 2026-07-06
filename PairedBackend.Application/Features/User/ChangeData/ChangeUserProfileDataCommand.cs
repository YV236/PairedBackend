using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.User.ChangeData;

public record ChangeUserProfileDataCommand(Guid UserId, string FirstName, string LastName, string UserName) : IRequest<Result>;
