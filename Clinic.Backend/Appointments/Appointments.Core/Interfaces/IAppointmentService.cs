namespace Appointments.Core.Interfaces;

public interface IAppointmentService
{
    public Task AddAppointmentAsync(DateTime date);
    public Task ApproveAppointmentAsync(string id);
}