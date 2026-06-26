using MediatR;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.Auth.RegisterUser;

public record RegisterUserCommand
(
    string Email,
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber
) : IRequest<Result<Guid>>;
