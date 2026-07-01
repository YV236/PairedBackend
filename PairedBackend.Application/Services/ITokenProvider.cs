using PairedBackend.Domain.Shared;
using System.Security.Claims;

namespace PairedBackend.Application.Services;

public interface ITokenProvider
{
    public string GenerateAccessToken(Guid userId, string email, Guid sessionId);

    public string GenerateRefreshToken();
}
