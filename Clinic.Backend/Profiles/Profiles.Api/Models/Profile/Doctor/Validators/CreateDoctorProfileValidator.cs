using FluentValidation;
using Profiles.Api.Models.Profile.Doctor.Requests;
using Profiles.Core.Enums;

namespace Profiles.Api.Models.Profile.Doctor.Validators;

public class CreateDoctorProfileValidator : AbstractValidator<CreateDoctorProfileRequest>
{
    public CreateDoctorProfileValidator()
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
            .Must(ValidateDateOfBirth).WithMessage("Doctor should be equal or over 18 years old");

        RuleFor(x => x.CareerStartYear)
            .NotNull().WithMessage("Career start year can't be null")
            .NotEmpty().WithMessage("Career start year can't be empty")
            .GreaterThan(x=>x.DateOfBirth.Year + 17).WithMessage("career start year must be greater than the year the doctor turned 18")
            .LessThan(DateTime.UtcNow.Year + 1).WithMessage("Career start year should be equal or lower current year");

        RuleFor(x => x.Status)
            .NotNull().WithMessage("Status can't be null")
            .NotEmpty().WithMessage("Status can't be empty")
            .IsInEnum().WithMessage("'Status' contains an invalid value.");
    }
    private bool ValidateDateOfBirth(DateTime dateOfBirth) => dateOfBirth <= DateTime.UtcNow.AddYears(-18);
}