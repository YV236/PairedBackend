using FluentValidation;

namespace PairedBackend.Application.Features.Sessions.RevokeSession;

internal class RevokeSessionCommandValidator : AbstractValidator<RevokeSessionCommand>
{
    public RevokeSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty).WithMessage("User ID cannot be an empty GUID.");

        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("Current session ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Session ID cannot be an empty GUID.");
    }
}
