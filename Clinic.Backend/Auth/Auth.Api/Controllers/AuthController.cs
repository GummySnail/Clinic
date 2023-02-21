using Auth.Api.Models.Auth.Requests;
using Auth.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUpAsync([FromBody] SignUpRequest request)
    {
        await _authService.SignUpAsync(request.Email, request.Password);

        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        var result = await _authService.SignInAsync(request.Email, request.Password);

        return Ok(result);
    }
}