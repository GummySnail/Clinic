using FluentValidation;
using Services.Api.Models.Service.Requests;

namespace Services.Api.Models.Service.Validators;

public class CreateServiceValidator : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.ServiceName)
            .NotNull().WithMessage("Service name can't be null")
            .NotEmpty().WithMessage("Service name can't be empty");
        
        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price can't be null")
            .NotEmpty().WithMessage("Price can't be empty")
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.ServiceCategory)
            .NotNull().WithMessage("Service category can't be null")
            .NotEmpty().WithMessage("Service category can't be empty")
            .IsInEnum();
        
    }
}