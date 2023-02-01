namespace Appointments.Core.Interfaces;

public interface IAppointmentService
{
    public Task AddAppointmentAsync(string patientId, string doctorId, string serviceId, DateTime date);
    public Task ApproveAppointmentAsync(string appointmentId);
    public Task CancelAppointmentAsync(string appointmentId);
    public Task CreateAppointmentResultAsync(string appointmentId, string complaints, string conclusion, string recommendations);
    public Task EditAppointmentResultAsync(string resultId, string complaints, string conclusion, string recommendations);
}