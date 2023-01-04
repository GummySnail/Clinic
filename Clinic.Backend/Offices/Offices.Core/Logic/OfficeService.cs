using Offices.Core.Entities;
using Offices.Core.Interfaces.Data.Repositories;
using Offices.Core.Logic.Exceptions;
using Offices.Core.Logic.Responses;

namespace Offices.Core.Logic;

public class OfficeService
{
    private readonly IOfficeRepository _officeRepository;

    public OfficeService(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<ICollection<OfficesResponse>> GetOfficesCollectionAsync()
    {
        var offices = await _officeRepository.GetOfficesCollectionAsync();
        
        var result = await _officeRepository.MappingToCollectionOfficesResponse(offices);

        return result;
    }

    public async Task CreateAsync(string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber, bool isActive)
    {
        var office = new Office(city, street, houseNumber, officeNumber, registryPhoneNumber, isActive);
        await _officeRepository.CreateAsync(office);
    }

    public async Task<OfficeResponse> GetOfficeByIdAsync(string id)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(id);

        if (office is null)
        {
            throw new NotFoundException("Office is not exist");
        }
        
        var result = await _officeRepository.MappingToOfficeResponse(office);

        if (result is null)
        {
            throw new Exception("Exception during mapping");
        }
        
        return result;
    }

    public async Task ChangeOfficeStatusAsync(string id, bool isActive)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(id);

        if (office is null)
        {
            throw new NotFoundException("Office is not exist");
        }

        await _officeRepository.ChangeOfficeStatusAsync(id, isActive);
    }

    public async Task EditOfficeAsync(string id, string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber, bool isActive)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(id);

        if (office is null)
        {
            throw new NotFoundException("Office is not exist");
        }

        var newOffice = new Office(city, street, houseNumber, officeNumber, registryPhoneNumber, isActive);
        
        await _officeRepository.EditOfficeAsync(id, newOffice);
    }
}