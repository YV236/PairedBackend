using Microsoft.AspNetCore.Identity;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;
using PairedBackend.Infrastructure.Identity;

namespace PairedBackend.Infrastructure.Services;

internal class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<Result<Guid>> RegisterAsync(string email, string username, string password, string firstName, string lastName, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = username,
            PasswordHash = password,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return Result.Success(user.Id);
        }
        var identityError = result.Errors.FirstOrDefault();

        if (identityError is null)
        {
            return Result.Failure<Guid>(Error.Unexpected);
        }

        var error = new Error(identityError.Code, identityError.Description, ErrorType.ValidationError);

        return Result.Failure<Guid>(error);
    }
}
