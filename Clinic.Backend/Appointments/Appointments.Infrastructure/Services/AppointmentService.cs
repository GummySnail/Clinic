using Appointments.Core.Entities;
using Appointments.Core.Interfaces;
using Appointments.Infrastructure.Data;

namespace Appointments.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppointmentsDbContext _context;

    public AppointmentService(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAppointmentAsync(DateTime date, bool IsApproved)
    {
        var appointment = new Appointment(date, IsApproved);

        await _context.Appointments.AddAsync(appointment);

        await _context.SaveChangesAsync();
    }
}