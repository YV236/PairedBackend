using Microsoft.EntityFrameworkCore;
using PairedBackend.Contracts.Responses.User;
using PairedBackend.Application.Services;
using PairedBackend.Contracts.Requests.User;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Persistence;

namespace PairedBackend.Infrastructure.Services;

internal class UserService(AppDbContext context) : IUserService
{
    public async Task<Result<UserProfileData>> GetUserProfileDataAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userProfileData = await context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserProfileData(
                u.UserName ?? string.Empty,
                u.Email ?? string.Empty,
                u.PhoneNumber ?? string.Empty,
                u.TwoFactorEnabled,
                u.FirstName,
                u.LastName,
                u.UsedPlatforms
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (userProfileData == null)
        {
            return Result.Failure<UserProfileData>(new Error("User.NotFound", "User not found", ErrorType.NotFound));
        }

        return Result.Success(userProfileData);
    }

    public async Task<Result> ChangeUserProfileData(
        Guid userId,
        ChangeProfileDataRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedUserName = request.UserName.ToUpperInvariant();

        var usersAffected = await context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FirstName, request.FirstName)
                .SetProperty(u => u.LastName, request.LastName)
                .SetProperty(u => u.UserName, request.UserName)
                .SetProperty(u => u.NormalizedUserName, normalizedUserName),
                cancellationToken);

        if (usersAffected == 0)
        {
            return Result.Failure(new Error("User.Error", "Could not change your data.", ErrorType.Conflict));
        }

        return Result.Success();
    }

    public Task<Result> AddMusicPlatform()
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangeProfilePicture()
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveMusicPlatform()
    {
        throw new NotImplementedException();
    }
}
