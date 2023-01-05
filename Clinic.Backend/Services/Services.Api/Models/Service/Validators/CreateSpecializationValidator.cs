using FluentValidation;
using Services.Api.Models.Service.Requests;

namespace Services.Api.Models.Service.Validators;

public class CreateSpecializationValidator : AbstractValidator<CreateSpecializationRequest>
{
    public CreateSpecializationValidator()
    {
        RuleFor(x => x.SpecializationName)
            .NotNull().WithMessage("Specialization name can't be null")
            .NotEmpty().WithMessage("Specialization name can't be empty");
    }
}