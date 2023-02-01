using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Documents.Core.Dto;
using Documents.Core.Entities;
using Documents.Core.Exceptions;
using Documents.Core.Interfaces.Services;
using Documents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Documents.Infrastructure.Services;

public class AzureService : IAzureService
{
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;
    private readonly DocumentsDbContext _context;

    public AzureService(IConfiguration config, DocumentsDbContext context)
    {
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
    
    public async Task UploadAppointmentResultDocumentAsync(byte[] bytes, string resultId)
    {
        BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);
        try
        {
            var blobClient = client.GetBlobClient($"{resultId}.pdf");

            using MemoryStream ms = new MemoryStream(bytes);
            await blobClient.UploadAsync(ms);
            var fileUri = blobClient.Uri;
            var document = new Document { Url = fileUri.ToString() };
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            throw new FileAlreadyExistException("File with that name already exists");
        }
    }

    public async Task<BlobDto> DownloadAsync(string blobFilename)
    {
        BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        BlobClient file = client.GetBlobClient(blobFilename);

        if (await file.ExistsAsync() == false)
        {
            throw new NotFoundException("File with that name does not exist");
        }
        
        var data = await file.OpenReadAsync();
        Stream blobContent = data;

        var content = await file.DownloadContentAsync();

        string name = blobFilename;
        string contentType = content.Value.Details.ContentType;

        return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
    }
}