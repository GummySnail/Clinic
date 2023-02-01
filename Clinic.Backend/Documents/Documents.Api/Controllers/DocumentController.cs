using Documents.Core.Dto;
using Documents.Core.Interfaces.Services;
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

    [HttpGet("{filename}")]
    public async Task<IActionResult> Get(string filename)
    {
        BlobDto file = await _azureService.DownloadAsync(filename);

        return File(file.Content, file.ContentType, file.Name);
    }
}