﻿using Microsoft.AspNetCore.Http;
using Offices.Core.Responses;

namespace Offices.Core.Interfaces.Services;

public interface IOfficeService
{
    public Task<ICollection<OfficeCollectionResponse>> GetOfficesCollectionAsync();
    public Task CreateOfficeAsync(string city, string street, string houseNumber, string officeNumber,
        string registryPhoneNumber, bool isActive, IFormFile? officePhoto);

    public Task<OfficeResponse> GetOfficeByIdAsync(string officeId);
    public Task UpdateOfficeStatusAsync(string officeId);

    public Task EditOfficeAsync(string officeId, string city, string street, string houseNumber, string officeNumber,
        string registryPhoneNumber, bool isActive, IFormFile? officePhoto);
}