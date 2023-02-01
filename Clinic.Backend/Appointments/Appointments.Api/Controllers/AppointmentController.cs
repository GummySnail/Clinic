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
    public async Task<ActionResult> AddAppointmentAsync([FromBody] CreateAppointmentRequest request)
    {
        await _appointmentService.AddAppointmentAsync(request.PatientId, request.DoctorId, request.ServiceId,request.AppointmentDate);

        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("approve-appointment/{appointmentId}")]
    public async Task<ActionResult> ApproveAppointmentAsync([FromRoute] string appointmentId)
    {
        await _appointmentService.ApproveAppointmentAsync(appointmentId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("cancel-appointment/{appointmentId}")]
    public async Task<ActionResult> CancelAppointmentAsync([FromRoute] string appointmentId)
    {
        await _appointmentService.CancelAppointmentAsync(appointmentId);

        return NoContent();
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpPost("create-appointment-result/{appointmentId}")]
    public async Task<ActionResult> CreateAppointmentResultAsync([FromRoute] string appointmentId,
        [FromBody] AppointmentResultRequest request)
    {
        await _appointmentService.CreateAppointmentResultAsync(appointmentId, request.Complaints,
                request.Conclusion, request.Recommendations);

        return NoContent();
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpPut("edit-result-information/{resultId}")]
    public async Task<ActionResult> EditAppointmentResultAsync([FromRoute] string resultId, [FromBody] AppointmentResultRequest request)
    {
        await _appointmentService.EditAppointmentResultAsync(resultId, request.Complaints, request.Conclusion, request.Recommendations);

        return NoContent();
    }
}