using MediatR;
using PairedBackend.Domain.Shared;
using System.Net;

namespace PairedBackend.Application.Features.Auth.LoginUser;

public record LoginUserCommand
(
    string UserName,
    string Email,
    string Password,
    string Device,
    string IpAddress
) : IRequest<Result<LoginResponse>>;
