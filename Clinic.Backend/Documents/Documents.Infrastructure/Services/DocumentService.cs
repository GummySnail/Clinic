using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Documents.Core.Dto;
using Documents.Core.Entities;
using Documents.Core.Interfaces.Services;
using Documents.Core.Responses;
using Documents.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Documents.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;
    private readonly ILogger<DocumentService> _logger;
    private readonly DocumentsDbContext _context;

    public DocumentService(IConfiguration config, ILogger<DocumentService> logger, DocumentsDbContext context)
    {
        _logger = logger;
        _context = context;
        _storageConnectionString = config.GetValue<string>("BlobConnectionString");
        _storageContainerName = config.GetValue<string>("BlobContainerName");
    }

    public async Task<BlobResponse> DeleteAsync(string blobFilename)
    {
        BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        BlobClient file = client.GetBlobClient(blobFilename);

        try
        {
            await file.DeleteAsync();
        }
        catch (RequestFailedException ex)
            when(ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            _logger.LogError($"File {blobFilename} was not found.");
            return new BlobResponse { Error = true, Status = $"File with name {blobFilename} not found." };
        }

        return new BlobResponse { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };
    }

    public async Task<List<BlobDto>> ListAsync()
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        List<BlobDto> files = new List<BlobDto>();

        await foreach (BlobItem file in container.GetBlobsAsync())
        {
            string uri = container.Uri.ToString();
            var name = file.Name;
            var fullUri = $"{uri}/{name}";

            files.Add(new BlobDto
            {
                Uri = fullUri,
                Name = name,
                ContentType = file.Properties.ContentType
            });
        }

        return files;
    }
    public async Task<BlobResponse> UploadProfilePhotoAsync(IFormFile file)
    {
        BlobResponse response = new();
        
        BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        try
        {
            BlobClient client = container.GetBlobClient(file.FileName);

            await using (Stream? data = file.OpenReadStream())
            {
                await client.UploadAsync(data);
            }
            
            var profilePhoto = new Photo { Url = client.Uri.AbsoluteUri };
            await _context.Photos.AddAsync(profilePhoto);

            await _context.SaveChangesAsync();

            response.Status = $"File {file.FileName} Uploaded Successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            _logger.LogError(
                $"File with name {file.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
            response.Status =
                $"File with name {file.FileName} already exists. Please use another name to store your file.";
            response.Error = true;
            return response;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
            response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
            response.Error = true;
            return response;
        }

        return response;
    }

    public async Task<BlobDto> DownloadAsync(string blobFilename)
    {
        BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        try
        {
            BlobClient file = client.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;

                var content = await file.DownloadContentAsync();

                string name = blobFilename;
                string contentType = content.Value.Details.ContentType;

                return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
            }
        }
        catch (RequestFailedException ex)
            when(ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            _logger.LogError($"File {blobFilename} was not found.");
        }

        return null;
    }
}