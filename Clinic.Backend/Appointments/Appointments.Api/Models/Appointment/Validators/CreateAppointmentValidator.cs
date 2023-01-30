using Appointments.Api.Models.Appointment.Requests;
using FluentValidation;

namespace Appointments.Api.Models.Appointment.Validators;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .NotNull().WithMessage("Appointment date can't be null")
            .NotEmpty().WithMessage("Appointment date can't be empty")
            .Must(ValidateAppointmentDate).WithMessage("Appointment date must be higher than the current one");

        RuleFor(x => x.PatientId)
            .NotNull().WithMessage("Patient id can't be null")
            .NotEmpty().WithMessage("Patient id can't be empty");
        
        RuleFor(x => x.DoctorId)
            .NotNull().WithMessage("Doctor id can't be null")
            .NotEmpty().WithMessage("Doctor id can't be empty");
        
        RuleFor(x => x.ServiceId)
            .NotNull().WithMessage("Service id can't be null")
            .NotEmpty().WithMessage("Service id can't be empty");
    }

    private bool ValidateAppointmentDate(DateTime appointmentDate) => appointmentDate > DateTime.UtcNow;
}