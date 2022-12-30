using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.Api.Models.Profile.Doctor.Requests;
using Profiles.Api.Models.Profile.Patient.Requests;
using Profiles.Api.Models.Profile.Receptionist.Requests;
using Profiles.Core.Logic.Profile;

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
}