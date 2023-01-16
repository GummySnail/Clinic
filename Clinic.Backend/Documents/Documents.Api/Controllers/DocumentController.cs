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
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }
    
    //[Authorize]
    [HttpPost("upload-profile-photo")]
    public async Task<ActionResult> UploadProfilePhoto(IFormFile file)
    {
        try
        {
            await _documentService.UploadProfilePhotoAsync(file);

            return NoContent();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
    
    //[Authorize]
    [HttpGet("get-profile-photos")]
    public async Task<IActionResult> Get()
    {
        List<BlobDto>? files = await _documentService.ListAsync();

        return Ok(files);
    }
    
    [HttpGet("{filename}")]
    public async Task<IActionResult> Get(string filename)
    {
        BlobDto? file = await _documentService.DownloadAsync(filename);

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
        BlobResponse response = await _documentService.DeleteAsync(filename);

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