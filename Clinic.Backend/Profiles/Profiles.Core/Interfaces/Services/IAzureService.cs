using Microsoft.AspNetCore.Http;

namespace Profiles.Core.Interfaces.Services;

public interface IAzureService
{
    public Task<string> UploadProfilePhotoAsync(IFormFile file);
}