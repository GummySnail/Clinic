using Microsoft.AspNetCore.Http;
using Offices.Core.Responses;

namespace Offices.Core.Interfaces.Services;

public interface IOfficeService
{
    public Task<ICollection<OfficeCollectionResponse>> GetOfficesCollectionAsync();
    public Task CreateAsync(string city, string street, string houseNumber, string officeNumber,
        string registryPhoneNumber, bool isActive, IFormFile? officePhoto);

    public Task<OfficeResponse> GetOfficeByIdAsync(string id);
    public Task ChangeOfficeStatusAsync(string id);

    public Task EditOfficeAsync(string id, string city, string street, string houseNumber, string officeNumber,
        string registryPhoneNumber, bool isActive, IFormFile? officePhoto);
}