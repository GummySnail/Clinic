﻿using Microsoft.AspNetCore.Authorization;
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
        try
        {
            await _serviceLogic
                .AddServiceAsync(request.ServiceName, request.Price, request.ServiceCategory, request.IsActive);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return NoContent();
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPost("create-specialization")]
    public async Task<ActionResult> CreateSpecialization([FromBody] SpecializationRequest request)
    {
        try
        {
            await _serviceLogic.AddSpecializationAsync(request.SpecializationName, request.IsActive, request.ServiceId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return NoContent();
    }
    
    //[Authorize(Roles="Receptionist")]
    [HttpPut("edit-specialization/{id}")]
    public async Task<ActionResult> EditSpecialization([FromRoute] string id, [FromBody] SpecializationRequest request)
    {
        try
        {
            await _serviceLogic.EditSpecializationAsync(id, request.SpecializationName, request.IsActive,
                request.ServiceId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return NoContent();
    }
}