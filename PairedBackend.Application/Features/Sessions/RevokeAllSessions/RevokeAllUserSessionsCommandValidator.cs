using FluentValidation;

namespace PairedBackend.Application.Features.Sessions.RevokeAllSessions;

internal class RevokeAllUserSessionsCommandValidator : AbstractValidator<RevokeAllUserSessionsCommand>
{
    public RevokeAllUserSessionsCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty).WithMessage("User ID cannot be an empty GUID.");

        RuleFor(x => x.CurrentSessionId)
            .NotEmpty().WithMessage("Current session ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Session ID cannot be an empty GUID.");
    }
}
