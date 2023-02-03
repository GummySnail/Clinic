using Microsoft.AspNetCore.Mvc;
using Services.Api.Models.Service.Requests;
using Services.Core.Enums;
using Services.Core.Interfaces.Services;
using Services.Core.Responses;

namespace Services.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IClinicService _clinicService;

    public ServiceController(IClinicService clinicService)
    {
        _clinicService = clinicService;
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-service")]
    public async Task<ActionResult> CreateServiceAsync([FromBody] ServiceRequest request)
    {
        await _clinicService
                .AddServiceAsync(request.ServiceName, request.Price, request.ServiceCategory, request.IsActive);
            
        return NoContent();
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPost("create-specialization")]
    public async Task<ActionResult> CreateSpecializationAsync([FromBody] SpecializationRequest request)
    {
        await _clinicService.AddSpecializationAsync(request.SpecializationName, request.IsActive, request.ServiceId);
            
        return NoContent();
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPut("{specializationId}/edit-specialization")]
    public async Task<ActionResult> EditSpecializationAsync([FromRoute] string specializationId, [FromBody] SpecializationRequest request)
    {
        await _clinicService.EditSpecializationAsync(specializationId, request.SpecializationName, request.IsActive,
                request.ServiceId);
        
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPut("{serviceId}/edit-service")]
    public async Task<ActionResult> EditServiceAsync([FromRoute] string serviceId, [FromBody] ServiceRequest request)
    {
        await _clinicService.EditServiceAsync(serviceId, request.ServiceName, request.Price, request.IsActive,
                request.ServiceCategory);

        return NoContent();
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("update-specialization-status/{specializationId}")]
    public async Task<ActionResult> UpdateSpecializationStatusAsync([FromRoute] string specializationId)
    {
        await _clinicService.UpdateSpecializationStatusAsync(specializationId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("update-service-status/{serviceId}")]
    public async Task<ActionResult> UpdateServiceStatusAsync([FromRoute] string serviceId)
    {
        await _clinicService.UpdateServiceStatusAsync(serviceId);

        return NoContent();
    }
    
    //[Authorize(Roles = "Patient")]
    [HttpGet("services/{category}")]
    public async Task<ActionResult<List<GetServicesResponse>>> GetServicesAsync([FromRoute] Category category)
    {
        var result = await _clinicService.GetServicesAsync(category);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("specializations")]
    public async Task<ActionResult<List<GetSpecializationsResponse>>> GetSpecializationsAsync()
    {
        var result = await _clinicService.GetSpecializationsAsync();

        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{serviceId}")]
    public async Task<ActionResult<GetServiceResponse>> GetServiceAsync([FromRoute] string serviceId)
    {
        var result = await _clinicService.GetServiceAsync(serviceId);

        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("specializations/{specializationId}")]
    public async Task<ActionResult> GetSpecializationAsync([FromRoute] string specializationId)
    {
        var result = await _clinicService.GetSpecializationAsync(specializationId);

        return Ok(result);
    }
}