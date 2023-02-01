using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Documents.Core.Dto;
using Documents.Core.Entities;
using Documents.Core.Interfaces.Services;
using Documents.Core.Responses;
using Documents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Documents.Infrastructure.Services;

public class AzureService : IAzureService
{
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;
    private readonly ILogger<AzureService> _logger;
    private readonly DocumentsDbContext _context;

    public AzureService(IConfiguration config, ILogger<AzureService> logger, DocumentsDbContext context)
    {
        _logger = logger;
        _context = context;
        _storageConnectionString = config.GetValue<string>("BlobConnectionString");
        _storageContainerName = config.GetValue<string>("BlobContainerName");
    }

    public async Task DeleteAsync(string blobFilename)
    {
        BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        BlobClient file = client.GetBlobClient(blobFilename);
        
        var document = await _context.Documents.SingleOrDefaultAsync(x => x.Url == file.Uri.ToString());
        
        await file.DeleteAsync();
        
        if (document is not null)
        {
            _context.Documents.Remove(document);
        }
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
    public async Task UploadAppointmentResultDocumentAsync(byte[] bytes, string resultId)
    {
        BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);
        var blobClient = client.GetBlobClient($"{resultId}.pdf");

        using MemoryStream ms = new MemoryStream(bytes);
        await blobClient.UploadAsync(ms);
        var fileUri = blobClient.Uri;
        var document = new Document { Url = fileUri.ToString() };
        await _context.Documents.AddAsync(document);
        await _context.SaveChangesAsync();
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