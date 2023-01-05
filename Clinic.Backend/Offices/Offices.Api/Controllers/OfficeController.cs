using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Offices.Api.Models.Office.Requests;
using Offices.Core.Logic;
using Offices.Core.Logic.Responses;

namespace Offices.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfficeController : ControllerBase
{
    private readonly OfficeService _officeService;

    public OfficeController(OfficeService officeService)
    {
        _officeService = officeService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<OfficesResponse>>> GetOffices()
    {
        ICollection<OfficesResponse> result;
        
        try
        {
            result = await _officeService.GetOfficesCollectionAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
        
        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult> Post(OfficeRequest request)
    {
        try
        {
            await _officeService.CreateAsync(request.City, request.Street, request.HouseNumber, request.OfficeNumber, request.RegistryPhoneNumber, request.IsActive);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
        
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{id}")]
    public async Task<ActionResult<OfficeResponse>> GetOffice([FromRoute] string id)
    {
        OfficeResponse result;
        try
        {
            result = await _officeService.GetOfficeByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPatch("{id}")]
    public async Task<ActionResult> ChangeOfficeStatus([FromRoute] string id,
        [FromBody] ChangeOfficeStatusRequest request)
    {
        try
        {
            await _officeService.ChangeOfficeStatusAsync(id, request.IsActive);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpPut("{id}")]
    public async Task<ActionResult> EditOffice([FromRoute] string id, [FromBody] OfficeRequest request)
    {
        try
        {
            await _officeService.EditOfficeAsync(id, request.City, request.Street, request.HouseNumber, request.OfficeNumber,
                request.RegistryPhoneNumber, request.IsActive);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return NoContent();
    }
}