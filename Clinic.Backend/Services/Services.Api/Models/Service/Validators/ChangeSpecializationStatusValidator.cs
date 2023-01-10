using FluentValidation;
using Services.Api.Models.Service.Requests;

namespace Services.Api.Models.Service.Validators;

public class ChangeSpecializationStatusValidator : AbstractValidator<ChangeSpecializationStatusRequest>
{
    public ChangeSpecializationStatusValidator()
    {
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("Status can't be null");
    }
}