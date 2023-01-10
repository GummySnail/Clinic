using FluentValidation;
using Services.Api.Models.Service.Requests;

namespace Services.Api.Models.Service.Validators;

public class GetServicesValidator : AbstractValidator<GetServicesRequest>
{
    public GetServicesValidator()
    {   
        RuleFor(x => x.Category)
            .NotNull().WithMessage("Service category can't be null")
            .NotEmpty().WithMessage("Service category can't be empty")
            .IsInEnum().WithMessage("'Service Category' contains an invalid value.");
    }
}