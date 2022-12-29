using Microsoft.AspNetCore.Mvc;
using Profiles.Api.Models.Profile.Patient.Requests;
using Profiles.Core.Logic.Profile;

namespace Profiles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly PatientService _patientService;

    public ProfileController(PatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("create-patient-profile")]
    public async Task<ActionResult> CreatePatientProfile([FromBody] CreatePatientProfileRequest request)
    {
        await _patientService.CreatePatientProfileAsync(request.FirstName, request.LastName, request?.MiddleName,
            request.DateOfBirth);
        return Ok();
    }
}