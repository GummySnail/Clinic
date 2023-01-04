using FluentValidation;
using Offices.Api.Models.Office.Requests;

namespace Offices.Api.Models.Office.Validators;

public class OfficeValidator : AbstractValidator<OfficeRequest>
{
    public OfficeValidator()
    {
        RuleFor(x => x.City)
            .NotNull().WithMessage("City can't be null")
            .NotEmpty().WithMessage("City can't be empty");

        RuleFor(x => x.Street)
            .NotNull().WithMessage("Street can't be null")
            .NotEmpty().WithMessage("Street can't be empty");
        
        RuleFor(x => x.HouseNumber)
            .NotNull().WithMessage("House number can't be null")
            .NotEmpty().WithMessage("House number can't be empty");
        
        RuleFor(x => x.OfficeNumber)
            .NotNull().WithMessage("Office number can't be null")
            .NotEmpty().WithMessage("Office number can't be empty");
        
        RuleFor(x => x.RegistryPhoneNumber)
            .NotNull().WithMessage("Registry phone number can't be null")
            .NotEmpty().WithMessage("Registry phone number can't be empty")
            .Matches(@"^[0-9]*$").WithMessage("Registry phone number should contains only numbers");
    }
}