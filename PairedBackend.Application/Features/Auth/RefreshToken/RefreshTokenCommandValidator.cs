using FluentValidation;

namespace PairedBackend.Application.Features.Auth.RefreshToken;

internal class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");

        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IP address is required.");

        RuleFor(x => x.Device)
            .NotEmpty().WithMessage("Device information is required.");
    }
}
