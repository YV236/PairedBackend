using PairedBackend.Application.Services;

namespace PairedBackend.Infrastructure.Services;

internal class TokenProvider : ITokenProvider
{
    public string GenerateAccessToken(Guid userId, string Email, Guid sessionId)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}
