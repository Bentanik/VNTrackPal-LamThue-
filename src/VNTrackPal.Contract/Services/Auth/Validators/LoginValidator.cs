using static VNTrackPal.Contract.Services.Auth.Query;

namespace VNTrackPal.Contract.Services.Auth.Validators;

public sealed class LoginValidator : AbstractValidator<LoginQuery>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Invalid email format.")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Email)
                            .MinimumLength(5).WithMessage("Email must be at least 2 characters long.")
                            .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters.");
                    });
            });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                    .MaximumLength(100).WithMessage("Password cannot be longer than 100 characters.");
            });
    }
}