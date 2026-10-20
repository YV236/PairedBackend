using PairedBackend.Contracts.Responses.User;
using PairedBackend.Contracts.Requests.User;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IUserService
{
    Task<Result<UserProfileData>> GetUserProfileDataAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result> ChangeUserProfileData(
        Guid userId,
        ChangeProfileDataRequest request,
        CancellationToken cancellationToken);

    Task<Result> ChangeProfilePicture();

    Task<Result> AddMusicPlatform();

    Task<Result> RemoveMusicPlatform();
}
