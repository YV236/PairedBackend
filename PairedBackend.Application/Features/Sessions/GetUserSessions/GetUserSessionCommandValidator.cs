using FluentValidation;

namespace PairedBackend.Application.Features.Sessions.GetUserSessions;

internal class GetUserSessionCommandValidator : AbstractValidator<GetUserSessionCommand>
{
    public GetUserSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty).WithMessage("User ID cannot be an empty GUID.");
    }
}
