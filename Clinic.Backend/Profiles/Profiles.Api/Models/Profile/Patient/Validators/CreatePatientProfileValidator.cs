using FluentValidation;
using Profiles.Api.Models.Profile.Patient.Requests;

namespace Profiles.Api.Models.Profile.Patient.Validators;

public class CreatePatientProfileValidator : AbstractValidator<CreatePatientProfileRequest>
{
    public CreatePatientProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("First Name can't be null")
            .NotEmpty().WithMessage("First Name can't be empty");

        RuleFor(x => x.LastName)
            .NotNull().WithMessage("Last Name can't be null")
            .NotEmpty().WithMessage("Last Name can't be empty");

        RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("Date of birth can't be null")
            .NotEmpty().WithMessage("Date of birth can't be empty")
            .Must(ValidateDateOfBirth).WithMessage("Date of birth mus be lower than the current one");

        RuleFor(x => x.PhoneNumber)
            .NotNull().WithMessage("Phone number can't be null")
            .NotEmpty().WithMessage("Phone number can't be empty")
            .Matches(@"^[0-9]*$").WithMessage("Phone number should contains only numbers");
    }

    private bool ValidateDateOfBirth(DateTime dateOfBirth) => dateOfBirth < DateTime.UtcNow;
}