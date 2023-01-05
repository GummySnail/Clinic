using Services.Core.Entities;
using Services.Core.Enums;

namespace Services.Core.Interfaces.Data.Repositories;

public interface IServiceCategoryRepository
{
    public Task<ServiceCategory?> GetServiceCategoryAsync(Category serviceCategory);
}