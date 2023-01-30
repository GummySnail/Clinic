namespace Appointments.Api.Models.Appointment.Requests;

public record CreateAppointmentRequest(string PatientId, string DoctorId, string ServiceId, DateTime AppointmentDate);