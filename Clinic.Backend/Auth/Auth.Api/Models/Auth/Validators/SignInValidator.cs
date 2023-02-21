using Auth.Api.Models.Auth.Requests;
using FluentValidation;

namespace Auth.Api.Models.Auth.Validators;

public class SignInValidator : AbstractValidator<SignInRequest>
{
    public SignInValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email can't be null")
            .NotEmpty().WithMessage("Email can't be empty")
            .EmailAddress().WithMessage("Is not a valid email address");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password can't be null")
            .NotEmpty().WithMessage("Password can't be empty")
            .MinimumLength(6).WithMessage("Minimum length 6")
            .MaximumLength(15).WithMessage("Maximum length 15");
    }
}