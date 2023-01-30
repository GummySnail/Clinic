using Microsoft.AspNetCore.Http;

namespace Offices.Core.Interfaces.Services;

public interface IAzureService
{
    public Task<string> UploadOfficePhotoAsync(IFormFile file);
}