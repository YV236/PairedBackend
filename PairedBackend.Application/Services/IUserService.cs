using PairedBackend.Application.Features.User.ProfileData;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IUserService
{
    public Task<Result<UserProfileData>> GetUserProfileDataAsync(Guid userId, CancellationToken cancellationToken);

    public Task<Result> ChangeUserProfileData(Guid userId, string firstName, string lastName, string userName, CancellationToken cancellationToken);

    public Task<Result> ChangeProfilePicture();

    public Task<Result> AddMusicPlatform();

    public Task<Result> RemoveMusicPlatform();
}
