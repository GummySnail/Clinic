using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Models.Service.Requests;
using Services.Core.Entities;
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
    public async Task<ActionResult> CreateService([FromBody] CreateServiceRequest request)
    {
        try
        {
            await _clinicService
                .AddServiceAsync(request.ServiceName, request.Price, request.ServiceCategory, request.IsActive);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPost("create-specialization")]
    public async Task<ActionResult> CreateSpecialization([FromBody] SpecializationRequest request)
    {
        try
        {
            await _clinicService.AddSpecializationAsync(request.SpecializationName, request.IsActive, request.ServiceId);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPut("edit-specialization/{id}")]
    public async Task<ActionResult> EditSpecialization([FromRoute] string id, [FromBody] SpecializationRequest request)
    {
        try
        {
            await _clinicService.EditSpecializationAsync(id, request.SpecializationName, request.IsActive,
                request.ServiceId);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Patient")]
    [HttpGet("view-services/{category}")]
    public async Task<ActionResult<List<GetServicesByCategoryResponse>>> GetServices([FromRoute] Category category)
    {
        try
        {
            var result = await _clinicService.GetServicesAsync(category);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}