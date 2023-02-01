using Appointments.Core.Entities;
using Appointments.Core.Exceptions;
using Appointments.Core.Interfaces;
using Appointments.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using Result = Appointments.Core.Entities.Result;

namespace Appointments.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppointmentsDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public AppointmentService(AppointmentsDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task AddAppointmentAsync(string patientId, string doctorId, string serviceId, DateTime date)
    {
        var appointment = new Appointment(doctorId, serviceId, patientId, date);

        await _context.Appointments.AddAsync(appointment);

        await _context.SaveChangesAsync();
    }

    public async Task ApproveAppointmentAsync(string appointmentId)
    {
        var appointment = await _context.Appointments.SingleOrDefaultAsync(x => x.Id == appointmentId);

        if (appointment is null)
        {
            throw new NotFoundException("Appointment is not exist");
        }
        
        appointment.IsApproved = true;

        await _context.SaveChangesAsync();
    }

    public async Task CancelAppointmentAsync(string appointmentId)
    {
        var appointment = await _context.Appointments.SingleOrDefaultAsync(x => x.Id == appointmentId);
        
        if (appointment is null)
        {
            throw new NotFoundException("Appointment is not exist");
        }
        
        _context.Appointments.RemoveRange(appointment);

        await _context.SaveChangesAsync();
    }

    public async Task CreateAppointmentResultAsync(string appointmentId, string complaints, string conclusion, string recommendations)
    {
        var appointment = await _context.Appointments.SingleOrDefaultAsync(x => x.Id == appointmentId);

        if (appointment is null)
        {
            throw new NotFoundException("Appointment is not exist");
        }

        var result = new Result(complaints, conclusion, recommendations, appointmentId);

        await _context.AddAsync(result);

        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new AppointmentResultCreated
        {
            ResultId = result.Id,
            Complaints = result.Complaints,
            Conclusion = result.Conclusion,
            Recommendations = result.Recommendations
        });
    }

    public async Task EditAppointmentResultAsync(string resultId, string complaints, string conclusion,
        string recommendations)
    {
        var appointmentResult = await _context.Results.SingleOrDefaultAsync(x => x.Id == resultId);

        if (appointmentResult is null)
        {
            throw new NotFoundException("Appointment result is not exist");
        }
        
        appointmentResult.Complaints = complaints;
        appointmentResult.Conclusion = conclusion;
        appointmentResult.Recommendations = recommendations;

        await _context.SaveChangesAsync();
        
        await _publishEndpoint.Publish(new AppointmentResultEdited
        {
            ResultId = appointmentResult.Id,
            Complaints = appointmentResult.Complaints,
            Conclusion = appointmentResult.Conclusion,
            Recommendations = appointmentResult.Recommendations
        });
    }
}