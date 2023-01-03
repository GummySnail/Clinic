using FluentValidation;
using Profiles.Api.Models.Profile.Doctor.Requests;

namespace Profiles.Api.Models.Profile.Doctor.Validators;

public class ChangeDoctorStatusValidator : AbstractValidator<ChangeDoctorStatusRequest>
{
    public ChangeDoctorStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotNull().WithMessage("Status can't be null")
            .NotEmpty().WithMessage("Status can't be empty")
            .IsInEnum().WithMessage("Status contains an invalid value");
    }
}