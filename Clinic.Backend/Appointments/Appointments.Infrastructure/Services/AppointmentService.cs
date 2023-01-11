using Appointments.Core.Entities;
using Appointments.Core.Interfaces;
using Appointments.Infrastructure.Data;
using Appointments.Infrastructure.Exceptions;

namespace Appointments.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppointmentsDbContext _context;

    public AppointmentService(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAppointmentAsync(DateTime date)
    {
        var appointment = new Appointment(date);

        await _context.Appointments.AddAsync(appointment);

        await _context.SaveChangesAsync();
    }

    public async Task ApproveAppointmentAsync(string id)
    {
        var appointment = _context.Appointments.SingleOrDefault(x => x.Id == id);

        if (appointment is null)
        {
            throw new NotFoundException("Appointment is not exist");
        }
        appointment.IsApproved = true;

        await _context.SaveChangesAsync();
    }
}