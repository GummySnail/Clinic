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
    [HttpPost("create-patient-profile")]
    public async Task<ActionResult> CreatePatientProfileAsync([FromForm] CreatePatientProfileRequest request)
    {
        
        await _profileService.CreatePatientProfileAsync(request.FirstName, request.LastName, request?.MiddleName,
                request.DateOfBirth, request.PhoneNumber, request?.ProfilePhoto);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-doctor-profile")]
    public async Task<ActionResult> CreateDoctorProfileAsync([FromForm] CreateDoctorProfileRequest request)
    {
        await _profileService.CreateDoctorProfileAsync(request.FirstName, request.LastName, request.MiddleName,
                request.DateOfBirth, request.CareerStartYear, request.Status, request?.ProfilePhoto);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-receptionist-profile")]
    public async Task<ActionResult> CreateReceptionistProfileAsync([FromForm] CreateReceptionistProfileRequest request)
    {
        await _profileService.CreateReceptionistProfileAsync(request.FirstName, request.LastName, request.MiddleName, request.ProfilePhoto);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-patient-profile-by-admin")]
    public async Task<ActionResult> CreatePatientByAdminAsync([FromBody] CreatePatientProfileByAdminRequest request)
    {
        await _profileService.CreatePatientProfileByAdminAsync(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth);
            
        return NoContent();
    }

    [HttpGet("doctors")]
    public async Task<ActionResult<ICollection<DoctorProfileResponse>>> GetDoctorsAtWorkAsync([FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetDoctorsAtWorkAsync(searchParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("get-doctors-by-admin")]
    public async Task<ActionResult<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(
        [FromQuery] SearchParams doctorParams)
    {
        var result = await _profileService.GetDoctorsByAdminAsync(doctorParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("receptionists")]
    public async Task<ActionResult<ICollection<ReceptionistProfileResponse>>> GetReceptionistsAsync(
        [FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetReceptionistsAsync(searchParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles ="Admin")]
    [HttpGet("get-patients-by-admin")]
    public async Task<ActionResult<PatientsProfileSearchByAdminResponse>> GetPatientsByAdminAsync(
        [FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetPatientsByAdminAsync(searchParams);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpGet("get-patient-profile-by-doctor/{patientId}")]
    public async Task<ActionResult<PatientProfileByDoctorResponse>> GetPatientProfileByDoctorAsync([FromRoute] string patientId)
    {
        var result = await _profileService.DoctorGetPatientProfileByIdAsync(patientId);

        return Ok(result);
    }

    [HttpGet("doctors/{doctorId}")]
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfileByIdAsync([FromRoute] string doctorId)
    {
        var result = await _profileService.GetDoctorProfileByIdAsync(doctorId);
        
        return Ok(result);
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpGet("get-patient-profile-by-admin/{patientId}")]
    public async Task<ActionResult<PatientProfileByAdminResponse>> GetPatientProfileByAdminAsync([FromRoute] string patientId)
    {
        var result = await _profileService.AdminGetPatientProfileByIdAsync(patientId);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("receptionists/{receptionistId}")]
    public async Task<ActionResult<ReceptionistProfileByIdResponse>> GetReceptionistProfileByIdAsync([FromRoute] string receptionistId)
    {
        var result = await _profileService.GetReceptionistProfileByIdAsync(receptionistId);
            
        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("patients/{patientId}")]
    public async Task<ActionResult> DeletePatientProfileAsync([FromRoute] string patientId)
    {
        await _profileService.DeletePatientProfileAsync(patientId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("receptionists/{receptionistId}")]
    public async Task<ActionResult> DeleteReceptionistProfileAsync([FromRoute] string receptionistId)
    {
        await _profileService.DeleteReceptionistProfileAsync(receptionistId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("update-doctor-status/{doctorId}")]
    public async Task<ActionResult> UpdateDoctorStatusAsync([FromRoute] string doctorId, [FromBody] ChangeDoctorStatusRequest request)
    {
        await _profileService.UpdateDoctorStatusAsync(doctorId, request.Status);
            
        return NoContent();
    }
}