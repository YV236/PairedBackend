using Microsoft.EntityFrameworkCore;
using PairedBackend.Application.Features.User.ProfileData;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Persistence;

namespace PairedBackend.Infrastructure.Services;

internal class UserService(AppDbContext appDbContext) : IUserService
{
    public async Task<Result<UserProfileData>> GetUserProfileDataAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<UserProfileData>(new Error("User.NotFound", "User not found", ErrorType.NotFound));
        }

        var userProfileData = new UserProfileData
        (
            UserName : user.UserName ?? string.Empty,
            Email : user.Email ?? string.Empty,
            PhoneNumber: user.PhoneNumber ?? string.Empty,
            TwoFactorEnabled: user.TwoFactorEnabled,
            FirstName: user.FirstName,
            LastName: user.LastName,
            UsedPlatforms : user.UsedPlatforms
        );

        return Result.Success(userProfileData);
    }

    public Task<Result> AddMusicPlatform()
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeProfilePicture()
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeUserInfo(string firstName, string lastName, string userName)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveMusicPlatform()
    {
        throw new NotImplementedException();
    }
}
