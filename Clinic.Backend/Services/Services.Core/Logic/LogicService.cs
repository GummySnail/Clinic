using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Interfaces.Data.Repositories;

namespace Services.Core.Logic;

public class LogicService
{
    private readonly IServiceRepository _serviceRepository;

    public LogicService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive)
    {
        var category = await _serviceRepository.GetServiceCategoryAsync(serviceCategory);

        var service = new Service(serviceName, price, category.Id, isActive);
        
        await _serviceRepository.AddServiceAsync(service);

        await _serviceRepository.SaveChangesAsync();
    }

    public async Task AddSpecializationAsync(string specializationName, bool isActive, string serviceId)
    {
        var specialization = new Specialization(specializationName, isActive);

        await _serviceRepository.AddSpecializationAsync(specialization, serviceId);

        await _serviceRepository.SaveChangesAsync();
    }
}