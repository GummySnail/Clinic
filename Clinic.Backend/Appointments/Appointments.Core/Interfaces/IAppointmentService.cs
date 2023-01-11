namespace Appointments.Core.Interfaces;

public interface IAppointmentService
{
    public Task AddAppointmentAsync(DateTime date, bool IsApproved);
}