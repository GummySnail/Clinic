using Documents.Core.Dto;
using Documents.Core.Interfaces.Services;
using Documents.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IAzureService _azureService;

    public DocumentController(IAzureService azureService)
    {
        _azureService = azureService;
    }

    //[Authorize]
    [HttpGet("get-profile-photos")]
    public async Task<IActionResult> Get()
    {
        List<BlobDto>? files = await _azureService.ListAsync();

        return Ok(files);
    }
    
    [HttpGet("{filename}")]
    public async Task<IActionResult> Get(string filename)
    {
        BlobDto? file = await _azureService.DownloadAsync(filename);

        if (file == null)
        {
            throw new Exception();
        }
        else
        {
            return File(file.Content, file.ContentType, file.Name);
        }
    }

    [HttpDelete("filename")]
    public async Task<IActionResult> Delete(string filename)
    {
        BlobResponse response = await _azureService.DeleteAsync(filename);

        if (response.Error == true)
        {
            throw new Exception();
        }
        else
        {
            return Ok(response.Status);
        }
    }
}