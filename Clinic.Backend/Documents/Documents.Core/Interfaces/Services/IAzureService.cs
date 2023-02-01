using Documents.Core.Dto;

namespace Documents.Core.Interfaces.Services;

public interface IAzureService
{
    public Task UploadAppointmentResultDocumentAsync(byte[] bytes, string resultId);
    public Task<BlobDto> DownloadAsync(string blobFilename);
    public Task DeleteAsync(string blobFilename);
}