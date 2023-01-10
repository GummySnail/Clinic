using FluentValidation;
using Services.Api.Models.Service.Requests;

namespace Services.Api.Models.Service.Validators;

public class SpecializationValidator : AbstractValidator<SpecializationRequest>
{
    public SpecializationValidator()
    {
        RuleFor(x => x.SpecializationName)
            .NotNull().WithMessage("Specialization name can't be null")
            .NotEmpty().WithMessage("Specialization name can't be empty");

        RuleFor(x => x.ServiceId)
            .NotNull().WithMessage("Service id can't be null")
            .NotEmpty().WithMessage("Service id can't be empty");
        
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("Status can't be null");
    }
}