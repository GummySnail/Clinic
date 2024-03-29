﻿using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Profiles.Core.Exceptions;
using Profiles.Core.Interfaces.Services;

namespace Profiles.Infrastructure.Services;

public class AzureService : IAzureService
{
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;

    public AzureService(IConfiguration config)
    {
        _storageConnectionString = config.GetValue<string>("BlobConnectionString");
        _storageContainerName = config.GetValue<string>("BlobContainerName");
    }

    public async Task<string> UploadProfilePhotoAsync(IFormFile file)
    {
        BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
        
        try
        {
            BlobClient client = container.GetBlobClient(file.FileName);

            await using (Stream? data = file.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            var profilePhotoUrl = client.Uri.ToString();
            
            return profilePhotoUrl;
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            throw new FileAlreadyExistException("File with that name already exists");
        }
    }
}