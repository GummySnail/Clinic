using Auth.Api.Extensions;
using Auth.Api.Models.Token.Requests;
using Auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var  result = await _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenRequest request)
    {
        await _tokenService.RevokeTokenAsync(User.GetId(), request.RefreshToken);
        return NoContent();
    }
}