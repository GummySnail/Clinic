using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Models.Service.Requests;
using Services.Core.Logic;

namespace Services.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly LogicService _serviceLogic;

    public ServiceController(LogicService serviceLogic)
    {
        _serviceLogic = serviceLogic;
    }

    //[Authorize(Roles = "Receptionist")]
    [HttpPost("create-service")]
    public async Task<ActionResult> CreateService([FromBody] CreateServiceRequest request)
    {
        await _serviceLogic.AddServiceAsync(request.ServiceName, request.Price, request.ServiceCategory,
            request.IsActive);

        return NoContent();
    }

}