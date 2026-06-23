using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Infrastructure.Services;

internal class UserService : IUserService
{
    public Task<Result<Guid>> RegisterAsync(string email, string username, string password, string firstName, string lastName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
