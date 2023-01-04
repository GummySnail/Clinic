using System.Collections;
using Offices.Core.Entities;
using Offices.Core.Logic.Responses;

namespace Offices.Core.Interfaces.Data.Repositories;

public interface IOfficeRepository
{
    public Task CreateAsync(Office office);
    public Task<List<Office>> GetOfficesCollectionAsync();
    public Task<Office> GetOfficeByIdAsync(string id);
    public Task ChangeOfficeStatusAsync(string id, bool isActive);
    public Task EditOfficeAsync(string id, Office newOffice);
    public Task<OfficeResponse> MappingToOfficeResponse(Office office);
    public Task<ICollection<OfficesResponse>> MappingToCollectionOfficesResponse(List<Office> offices);
}