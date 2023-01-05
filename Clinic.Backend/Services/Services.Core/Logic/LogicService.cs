using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Interfaces.Data.Repositories;
using Services.Core.Logic.Exceptions;

namespace Services.Core.Logic;

public class LogicService
{
    private readonly IRepositoryManager _repositoryManager;

    public LogicService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive)
    {
        var category = await _repositoryManager.ServiceCategoryRepository.GetServiceCategoryAsync(serviceCategory);

        if (category is null)
        {
            throw new NotFoundException("category is not exist");
        }
        
        var service = new Service(serviceName, price, category.Id, isActive);
        
        await _repositoryManager.ServiceRepository.AddServiceAsync(service);

        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseException("Unable to create service");
        }
    }
}