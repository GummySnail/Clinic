using Microsoft.AspNetCore.Mvc;
using Offices.Api.Models.Office.Requests;
using Offices.Core.Interfaces.Services;
using Offices.Core.Responses;

namespace Offices.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfficeController : ControllerBase
{
    private readonly IOfficeService _officeService;

    public OfficeController(IOfficeService officeService)
    {
        _officeService = officeService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<OfficeCollectionResponse>>> GetOfficesAsync()
    {
        var result = await _officeService.GetOfficesCollectionAsync();

        return Ok(result);
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{officeId}")]
    public async Task<ActionResult<OfficeResponse>> GetOfficeAsync([FromRoute] string officeId)
    {
        var result = await _officeService.GetOfficeByIdAsync(officeId);
            
        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult> CreateOfficeAsync([FromForm] OfficeRequest request)
    {
        await _officeService.CreateOfficeAsync(request.City, request.Street, request.HouseNumber, request.OfficeNumber, request.RegistryPhoneNumber, request.IsActive, request?.officePhoto);
            
        return NoContent();
    }
    

    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("{officeId}")]
    public async Task<ActionResult> UpdateOfficeStatusAsync([FromRoute] string officeId)
    {
        await _officeService.UpdateOfficeStatusAsync(officeId);
            
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPut("{officeId}/edit")]
    public async Task<ActionResult> EditOfficeAsync([FromRoute] string officeId, [FromForm] OfficeRequest request)
    {
        await _officeService.EditOfficeAsync(officeId, request.City, request.Street, request.HouseNumber, request.OfficeNumber,
                request.RegistryPhoneNumber, request.IsActive, request.officePhoto);
            
        return NoContent();
    }
}