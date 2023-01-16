using Documents.Core.Dto;
using Documents.Core.Responses;
using Microsoft.AspNetCore.Http;

namespace Documents.Core.Interfaces.Services;

public interface IDocumentService
{
    public Task<BlobResponse> UploadProfilePhotoAsync(IFormFile file);
    public Task<BlobDto> DownloadAsync(string blobFilename);
    public Task<BlobResponse> DeleteAsync(string blobFilename);
    Task<List<BlobDto>> ListAsync();
}