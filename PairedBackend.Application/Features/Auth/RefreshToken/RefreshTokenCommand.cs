using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken,
    string IpAddress,
    string Device
) : IRequest<Result<RefreshTokenResponse>>;
