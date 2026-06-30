using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Services;

public interface IEmailService
{
    public Task<Result> SendConfirmationEmail(string userEmail);

    public Task<Result> SendResetEmail(string userEmail);
}
