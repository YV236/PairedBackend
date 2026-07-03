using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.User.ProfileData;

public record GetUserProfileDataCommand(Guid UserId) : IRequest<Result<UserProfileData>>;
