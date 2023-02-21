using Auth.Api.Models.Token.Requests;
using FluentValidation;

namespace Auth.Api.Models.Token.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotNull().WithMessage("Access token can't be null")
            .NotEmpty().WithMessage("Refresh token can't be empty");

        RuleFor(x => x.RefreshToken)
            .NotNull().WithMessage("Refresh token can't be null")
            .NotEmpty().WithMessage("Refresh token can't be empty");
    }
}