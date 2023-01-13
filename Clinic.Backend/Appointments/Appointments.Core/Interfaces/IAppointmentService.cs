namespace Appointments.Core.Interfaces;

public interface IAppointmentService
{
    public Task AddAppointmentAsync(DateTime date);
    public Task ApproveAppointmentAsync(string id);
    public Task CancelAppointmentAsync(string id);
    public Task CreateAppointmentResultAsync(string appointmentId, string complaints, string conclusion,
        string recommendations);
    public Task EditAppointmentResultAsync(string appointmentResultId, string complaints, string conclusion,
        string recommendations);
}