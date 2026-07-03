using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.User.ProfileData;

internal class GetUserProfileDataCommandHandler(IUserService userService) : IRequestHandler<GetUserProfileDataCommand, Result<UserProfileData>>
{
    public async Task<Result<UserProfileData>> Handle(GetUserProfileDataCommand request, CancellationToken cancellationToken)
    {
        return await userService.GetUserProfileDataAsync(request.UserId, cancellationToken);
    }
}
