using FluentValidation;
using Services.Api.Models.Service.Requests;

namespace Services.Api.Models.Service.Validators;

public class ServiceValidator : AbstractValidator<ServiceRequest>
{
    public 
        ServiceValidator()
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
            .IsInEnum().WithMessage("'Service Category' contains an invalid value.");
        
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("Status can't be null");
    }
}