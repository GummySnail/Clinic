using Documents.Core.Dto;
using Documents.Core.Responses;

namespace Documents.Core.Interfaces.Services;

public interface IAzureService
{
    public Task UploadAppointmentResultDocumentAsync(byte[] bytes, string resultId);
    public Task<BlobDto> DownloadAsync(string blobFilename);
    public Task<BlobResponse> DeleteAsync(string blobFilename);
    Task<List<BlobDto>> ListAsync();
}