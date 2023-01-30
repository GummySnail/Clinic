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
    public async Task<ActionResult> CreatePatientProfile([FromForm] CreatePatientProfileRequest request)
    {
        try
        {
            await _profileService.CreatePatientProfileAsync(request.FirstName, request.LastName, request?.MiddleName,
                request.DateOfBirth, request.PhoneNumber, request?.ProfilePhoto);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-doctor-profile")]
    public async Task<ActionResult> CreateDoctorProfile([FromForm] CreateDoctorProfileRequest request)
    {
        try
        {
            await _profileService.CreateDoctorProfileAsync(request.FirstName, request.LastName, request.MiddleName,
                request.DateOfBirth, request.CareerStartYear, request.Status, request?.ProfilePhoto);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-receptionist-profile")]
    public async Task<ActionResult> CreateReceptionistProfile([FromForm] CreateReceptionistProfileRequest request)
    {
        try
        {
            await _profileService.CreateReceptionistProfileAsync(request.FirstName, request.LastName, request.MiddleName, request.ProfilePhoto);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-patient's-profile-by-admin")]
    public async Task<ActionResult> CreatePatientByAdmin([FromBody] CreatePatientProfileByAdminRequest request)
    {
        try
        {
            await _profileService.CreatePatientProfileByAdminAsync(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

    }

    [HttpGet("view-doctors")]
    public async Task<ActionResult<ICollection<DoctorProfileResponse>>> GetDoctorsAtWork([FromQuery] SearchParams searchParams)
    {
        try
        {
            var result = await _profileService.GetDoctorsAtWorkAsync(searchParams);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("view-doctors-by-admin")]
    public async Task<ActionResult<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdmin(
        [FromQuery] SearchParams doctorParams)
    {
        try
        {
            var result = await _profileService.GetDoctorsByAdminAsync(doctorParams);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("view-receptionists")]
    public async Task<ActionResult<ICollection<ReceptionistProfileResponse>>> GetReceptionists(
        [FromQuery] SearchParams searchParams)
    {
        try
        {
            var result = await _profileService.GetReceptionistsAsync(searchParams);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles ="Admin")]
    [HttpGet("view-patients-by-admin")]
    public async Task<ActionResult<PatientsProfileSearchByAdminResponse>> GetPatientsByAdmin(
        [FromQuery] SearchParams searchParams)
    {
        try
        {
            var result = await _profileService.GetPatientsByAdminAsync(searchParams);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpGet("view-patient's-profile-by-doctor/{id}")]
    public async Task<ActionResult<PatientProfileByDoctorResponse>> GetPatientProfileById([FromRoute] string id)
    {
        try
        {
            var result = await _profileService.DoctorGetPatientProfileByIdAsync(id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    [HttpGet("view-doctor-information/{id}")]
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfileById([FromRoute] string id)
    {
        try
        {
            var result = await _profileService.GetDoctorProfileByIdAsync(id);
        
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpGet("view-patient's-profile-by-admin/{id}")]
    public async Task<ActionResult<PatientProfileByAdminResponse>> GetPatientsProfiles([FromRoute] string id)
    {
        try
        {
            var result = await _profileService.AdminGetPatientProfileByIdAsync(id);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("view-receptionist's-profile/{id}")]
    public async Task<ActionResult<ReceptionistProfileByIdResponse>> GetReceptionistProfileById([FromRoute] string id)
    {
        try
        {
            var result = await _profileService.GetReceptionistProfileByIdAsync(id);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("delete-patient's-profile/{id}")]
    public async Task<ActionResult> DeletePatientProfile([FromRoute] string id)
    {
        try
        {
            await _profileService.DeletePatientProfileAsync(id);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpDelete("delete-receptionist's-profile/{id}")]
    public async Task<ActionResult> DeleteReceptionistProfile([FromRoute] string id)
    {
        try
        {
            await _profileService.DeleteReceptionistProfileAsync(id);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("change-doctor's-status/{id}")]
    public async Task<ActionResult> ChangeDoctorStatus([FromRoute] string id, [FromBody] ChangeDoctorStatusRequest request)
    {
        try
        {
            await _profileService.ChangeDoctorStatusAsync(id, request.Status);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}