using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IUserService
{
    public Task<Result> ChangeUserInfo(string firstName, string lastName, string userName);

    public Task<Result> ChangeProfilePicture();

    public Task<Result> AddMusicPlatform();

    public Task<Result> RemoveMusicPlatform();
}
