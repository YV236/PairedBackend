using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IUserService
{
    Task<Result<Guid>> RegisterAsync(
        string email,
        string username,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken);
}
