using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.Api.Models.Profile.Doctor.Requests;
using Profiles.Api.Models.Profile.Patient.Requests;
using Profiles.Api.Models.Profile.Receptionist.Requests;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }
    
    //[Authorize]
    [HttpPost("create-patient-profile")]
    public async Task<ActionResult> CreatePatientProfile([FromBody] CreatePatientProfileRequest request)
    {
        await _profileService.CreatePatientProfileAsync(request.FirstName, request.LastName, request?.MiddleName,
            request.DateOfBirth, request.PhoneNumber);
     
        return Ok();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-doctor-profile")]
    public async Task<ActionResult> CreateDoctorProfile([FromBody] CreateDoctorProfileRequest request)
    {
        await _profileService.CreateDoctorProfileAsync(request.FirstName, request.LastName, request.MiddleName,
            request.DateOfBirth, request.CareerStartYear, request.Status);

        return Ok();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-receptionist-profile")]
    public async Task<ActionResult> CreateReceptionistProfile([FromBody] CreateReceptionistProfileRequest request)
    {
        await _profileService.CreateReceptionistProfileAsync(request.FirstName, request.LastName, request.MiddleName);

        return Ok();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-patient's-profile-by-admin")]
    public async Task<ActionResult> CreatePatientByAdmin([FromBody] CreatePatientProfileByAdminRequest request)
    {
        await _profileService.CreatePatientProfileByAdminAsync(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth);

        return Ok();
    }

    [HttpGet("view-doctors")]
    public async Task<ActionResult<ICollection<DoctorProfileResponse>>> GetDoctorsAtWork([FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetDoctorsAtWorkAsync(searchParams);

        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("view-doctors-by-admin")]
    public async Task<ActionResult<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdmin(
        [FromQuery] SearchParams doctorParams)
    {
        var result = await _profileService.GetDoctorsByAdminAsync(doctorParams);

        return Ok(result);
    }
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("view-receptionists")]
    public async Task<ActionResult<ICollection<ReceptionistProfileResponse>>> GetReceptionists(
        [FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetReceptionistsAsync(searchParams);

        return Ok(result);
    }
    
    //[Authorize(Roles ="Admin")]
    [HttpGet("view-patients-by-admin")]
    public async Task<ActionResult<PatientsProfileSearchByAdminResponse>> GetPatientsByAdmin(
        [FromQuery] SearchParams searchParams)
    {
        var result = await _profileService.GetPatientsByAdminAsync(searchParams);

        return Ok(result);
    }
    
    //[Authorize(Roles = "Doctor")]
    [HttpGet("view-patient's-profile-by-doctor/{id}")]
    public async Task<ActionResult<PatientProfileByDoctorResponse>> GetPatientProfileById(string id)
    {
        var result = await _profileService.DoctorGetPatientProfileByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("view-doctor-information/{id}")]
    public async Task<ActionResult<DoctorProfileResponse>> GetDoctorProfileById(string id)
    {
        var result = await _profileService.GetDoctorProfileByIdAsync(id);

        return Ok(result);
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpGet("view-patient's-profile-by-admin/{id}")]
    public async Task<ActionResult<PatientProfileByAdminResponse>> GetPatientsProfiles(string id)
    {
        var result = await _profileService.AdminGetPatientProfileByIdAsync(id);

        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("view-receptionist's-profile/{id}")]
    public async Task<ActionResult<ReceptionistProfileByIdResponse>> GetReceptionistProfileById(string id)
    {
        var result = await _profileService.GetReceptionistProfileByIdAsync(id);

        return Ok(result);
    }
}