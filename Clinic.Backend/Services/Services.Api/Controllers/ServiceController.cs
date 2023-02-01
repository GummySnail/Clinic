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
    [HttpPost("services")]
    public async Task<ActionResult> Create([FromBody] ServiceRequest request)
    {
        await _clinicService
            .AddServiceAsync(request.ServiceName, request.Price, request.ServiceCategory, request.IsActive);
        
        return NoContent();
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPost("specializations")]
    public async Task<ActionResult> Create([FromBody] SpecializationRequest request)
    {
        await _clinicService.AddSpecializationAsync(request.SpecializationName, request.IsActive, request.ServiceId);
        
        return NoContent();
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPut("{specializationId}/edit")]
    public async Task<ActionResult> Edit([FromRoute] string specializationId, [FromBody] SpecializationRequest request)
    {
        await _clinicService.EditSpecializationAsync(specializationId, request.SpecializationName, request.IsActive,
            request.ServiceId);
        
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPut("{serviceId}/edit")]
    public async Task<ActionResult> Edit([FromRoute] string serviceId, [FromBody] ServiceRequest request)
    {
        await _clinicService.EditServiceAsync(serviceId, request.ServiceName, request.Price, request.IsActive, request.ServiceCategory);

        return NoContent();
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("specializations/{specializationId}")]
    public async Task<ActionResult> UpdateSpecialization([FromRoute] string specializationId)
    {
        await _clinicService.ChangeSpecializationStatusAsync(specializationId);
            
        return NoContent(); 
        
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("services/{serviceId}")]
    public async Task<ActionResult> UpdateService([FromRoute] string serviceId)
    {
        await _clinicService.ChangeServiceStatusAsync(serviceId);
        
        return NoContent();
        
    }
    
    //[Authorize(Roles = "Patient")]
    [HttpGet("{serviceCategory}")]
    public async Task<ActionResult<List<GetServicesResponse>>> Get([FromRoute] Category category)
    {
        var result = await _clinicService.GetServicesAsync(category);
            
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet]
    public async Task<ActionResult<List<GetSpecializationsResponse>>> GetSpecializations()
    {
        var result = await _clinicService.GetSpecializationsAsync();

        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{serviceId}")]
    public async Task<ActionResult<GetServiceResponse>> GetService([FromRoute] string serviceId)
    {
        var result = await _clinicService.GetServiceAsync(serviceId);
        
        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{specializationId}")]
    public async Task<ActionResult> GetSpecialization([FromRoute] string specializationId)
    {
        var result = await _clinicService.GetSpecializationAsync(specializationId);

        return Ok(result);
    }
}