using Auth.Api.Models.Token.Requests;
using FluentValidation;

namespace Auth.Api.Models.Token.Validators;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenRequest>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotNull().WithMessage("Refresh token can't be null")
            .NotEmpty().WithMessage("Refresh token can't be empty");
    }
}