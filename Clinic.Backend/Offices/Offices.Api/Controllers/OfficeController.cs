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
        var result = await _officeService.GetOfficesCollectionAsync();

        return Ok(result);
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult> Post(CreateOfficeRequest request)
    {
        await _officeService.CreateAsync(request.City, request.Street, request.HouseNumber, request.OfficeNumber, request.RegistryPhoneNumber, request.IsActive);
        
        return NoContent();
    }
    
    //[Authorize(Roles = "Receptionist")]
    [HttpGet("{id}")]
    public async Task<ActionResult<OfficeResponse>> GetOffice([FromRoute] string id)
    {
        var result = await _officeService.GetOfficeByIdAsync(id);

        return Ok(result);
    }
}