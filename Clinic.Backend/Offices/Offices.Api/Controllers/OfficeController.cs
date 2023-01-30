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
    public async Task<ActionResult<ICollection<OfficeCollectionResponse>>> GetOffices()
    {
        try
        {
            var result = await _officeService.GetOfficesCollectionAsync();
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
        
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{id}")]
    public async Task<ActionResult<OfficeResponse>> GetOffice([FromRoute] string id)
    {
        try
        {
            var result = await _officeService.GetOfficeByIdAsync(id);
            
            return Ok(result);  
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult> Post([FromForm] OfficeRequest request)
    {
        try
        {
            await _officeService.CreateAsync(request.City, request.Street, request.HouseNumber, request.OfficeNumber, request.RegistryPhoneNumber, request.IsActive, request?.officePhoto);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    

    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("{id}")]
    public async Task<ActionResult> ChangeOfficeStatus([FromRoute] string id)
    {
        try
        {
            await _officeService.ChangeOfficeStatusAsync(id);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPut("{id}")]
    public async Task<ActionResult> EditOffice([FromRoute] string id, [FromForm] OfficeRequest request)
    {
        try
        {
            await _officeService.EditOfficeAsync(id, request.City, request.Street, request.HouseNumber, request.OfficeNumber,
                request.RegistryPhoneNumber, request.IsActive, request.officePhoto);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}