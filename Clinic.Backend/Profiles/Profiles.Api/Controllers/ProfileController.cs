using Microsoft.AspNetCore.Mvc;
using Profiles.Api.Models.Profile.Doctor.Requests;
using Profiles.Api.Models.Profile.Patient.Requests;
using Profiles.Api.Models.Profile.Receptionist.Requests;
using Profiles.Core.Interfaces.Services;
using Profiles.Core.Pagination;
using Profiles.Core.Responses;

namespace Profiles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    //[Authorize]
    [HttpPost("create-patient")]
    public async Task<ActionResult> Create([FromForm] CreatePatientProfileRequest request)
    {
        await _profileService.CreatePatientProfileAsync(request.FirstName, request.LastName, request?.MiddleName,
                request.DateOfBirth, request.PhoneNumber, request?.ProfilePhoto);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-doctor")]
    public async Task<ActionResult> Create([FromForm] CreateDoctorProfileRequest request)
    {
        await _profileService.CreateDoctorProfileAsync(request.FirstName, request.LastName, request.MiddleName,
                request.DateOfBirth, request.CareerStartYear, request.Status, request?.ProfilePhoto);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-receptionist")]
    public async Task<ActionResult> Create([FromForm] CreateReceptionistProfileRequest request)
    {
        await _profileService.CreateReceptionistProfileAsync(request.FirstName, request.LastName, request.MiddleName, request.ProfilePhoto);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-patient-by-admin")]
    public async Task<ActionResult> CreatePatientByAdmin([FromBody] CreatePatientProfileByAdminRequest request)
    {
        await _profileService.CreatePatientProfileByAdminAsync(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth);
            
        return NoContent();
    }

    [HttpGet("doctors")]
    public async Task<ActionResult<ICollection<DoctorProfileResponse>>> GetDoctors([FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetDoctorsAtWorkAsync(searchParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("doctors-by-admin")]
    public async Task<ActionResult<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdmin(
        [FromQuery] SearchParams doctorParams)
    {
        var result = await _profileService.GetDoctorsByAdminAsync(doctorParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("receptionists")]
    public async Task<ActionResult<ICollection<ReceptionistProfileResponse>>> GetReceptionists(
        [FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetReceptionistsAsync(searchParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles ="Admin")]
    [HttpGet("patients")]
    public async Task<ActionResult<PatientsProfileSearchByAdminResponse>> GetPatientsByAdmin(
        [FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetPatientsByAdminAsync(searchParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpGet("patients/{patientId}")]
    public async Task<ActionResult<PatientProfileByDoctorResponse>> GetPatientProfileById([FromRoute] string patientId)
    {
        var result = await _profileService.DoctorGetPatientProfileByIdAsync(patientId);

        return Ok(result);
    }

    [HttpGet("doctors/{doctorId}")]
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfileById([FromRoute] string doctorId)
    {
        var result = await _profileService.GetDoctorProfileByIdAsync(doctorId);
        
        return Ok(result);
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpGet("patients/admin/{patientId}")]
    public async Task<ActionResult<PatientProfileByAdminResponse>> GetPatientProfileByAdmin([FromRoute] string patientId)
    {
        var result = await _profileService.AdminGetPatientProfileByIdAsync(patientId);
        
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("receptionists/{receptionistId}")]
    public async Task<ActionResult<ReceptionistProfileByIdResponse>> GetReceptionistProfileById([FromRoute] string receptionistId)
    {
        var result = await _profileService.GetReceptionistProfileByIdAsync(receptionistId);
            
        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("patients/{patientId}")]
    public async Task<ActionResult> DestroyPatientProfile([FromRoute] string patientId)
    {
        await _profileService.DeletePatientProfileAsync(patientId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("receptionists/{receptionistId}")]
    public async Task<ActionResult> DestroyReceptionistProfile([FromRoute] string receptionistId)
    {
        await _profileService.DeleteReceptionistProfileAsync(receptionistId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("doctors/{doctorId}")]
    public async Task<ActionResult> UpdateDoctorStatus([FromRoute] string doctorId, [FromBody] ChangeDoctorStatusRequest request)
    {
        await _profileService.ChangeDoctorStatusAsync(doctorId, request.Status);
            
        return NoContent();
    }
}