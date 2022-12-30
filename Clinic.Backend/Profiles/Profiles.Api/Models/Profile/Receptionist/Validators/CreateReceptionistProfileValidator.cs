using FluentValidation;
using Profiles.Api.Models.Profile.Receptionist.Requests;

namespace Profiles.Api.Models.Profile.Receptionist.Validators;

public class CreateReceptionistProfileValidator : AbstractValidator<CreateReceptionistProfileRequest>
{
    public CreateReceptionistProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("First Name can't be null")
            .NotEmpty().WithMessage("First Name can't be empty");

        RuleFor(x => x.LastName)
            .NotNull().WithMessage("Last Name can't be null")
            .NotEmpty().WithMessage("Last Name can't be empty");
    }
}