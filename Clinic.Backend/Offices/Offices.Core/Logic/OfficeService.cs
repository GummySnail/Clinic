using Offices.Core.Entities;
using Offices.Core.Interfaces.Data.Repositories;
using Offices.Core.Logic.Responses;

namespace Offices.Core.Logic;

public class OfficeService
{
    private readonly IOfficeRepository _officeRepository;

    public OfficeService(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<ICollection<OfficesResponse>> GetAsync()
    {
        var offices = await _officeRepository.GetAsync();
        
        var result = await _officeRepository.MappingToCollectionOfficesResponse(offices);

        return result;
    }

    public async Task CreateAsync(string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber, bool isActive)
    {
        var office = new Office(city, street, houseNumber, officeNumber, registryPhoneNumber, isActive);
        await _officeRepository.CreateAsync(office);
    }
}