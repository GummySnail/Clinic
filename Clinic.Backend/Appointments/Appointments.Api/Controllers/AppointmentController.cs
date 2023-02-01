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
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        await _appointmentService.AddAppointmentAsync(request.PatientId, request.DoctorId, request.ServiceId,
            request.AppointmentDate);

        return NoContent();
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("{appointmentId}")]
    public async Task<ActionResult> Update([FromRoute] string id)
    {
        await _appointmentService.ApproveAppointmentAsync(id);

        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("{appointmentId}")]
    public async Task<ActionResult> Destroy([FromRoute] string id)
    {
        await _appointmentService.CancelAppointmentAsync(id);

        return NoContent();
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpPost("{appointmentId}")]
    public async Task<ActionResult> Create([FromRoute] string appointmentId,
        [FromBody] AppointmentResultRequest request)
    {
        await _appointmentService.CreateAppointmentResultAsync(appointmentId, request.Complaints,
                request.Conclusion, request.Recommendations);

        return NoContent();
    }

    //[Authorize(Roles = "Doctor")]
    [HttpPut("{resultId}/edit")]
    public async Task<ActionResult> Edit([FromRoute] string resultId, [FromBody] AppointmentResultRequest request)
    {
        await _appointmentService.EditAppointmentResultAsync(resultId, request.Complaints, request.Conclusion, request.Recommendations);

        return NoContent();
    }
}