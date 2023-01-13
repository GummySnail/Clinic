using Appointments.Api.Models.Appointment.Requests;
using FluentValidation;

namespace Appointments.Api.Models.Appointment.Validators;

public class AppointmentResultValidator : AbstractValidator<AppointmentResultRequest>
{
    public AppointmentResultValidator()
    {
        RuleFor(x => x.Complaints)
            .NotNull().WithMessage("Complaints can't be null")
            .NotEmpty().WithMessage("Complaints can't be empty");

        RuleFor(x => x.Conclusion)
            .NotNull().WithMessage("Conclusion can't be null")
            .NotEmpty().WithMessage("Conclusion can't be empty");

        RuleFor(x => x.Recommendations)
            .NotNull().WithMessage("Recommendations can't be null")
            .NotEmpty().WithMessage("Recommendations can't be empty");
    }
}