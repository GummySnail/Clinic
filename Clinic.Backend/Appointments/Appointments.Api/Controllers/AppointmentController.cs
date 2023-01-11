using Appointments.Api.Models.Appointment.Requests;
using Appointments.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    //[Authorize(Roles = "Patient")]
    [HttpPost("create-appointment")]
    public async Task<ActionResult> AddAppointment([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            await _appointmentService.AddAppointmentAsync(request.AppointmentDate, request.IsApproved);

            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}