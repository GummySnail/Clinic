using Services.Core.Entities;
using Services.Core.Enums;

namespace Services.Core.Interfaces.Data.Repositories;

public interface IServiceRepository
{
    public Task AddServiceAsync(Service service);
    public Task<ServiceCategory?> GetServiceCategoryAsync(Category serviceCategory);
    public Task AddSpecializationAsync(Specialization specialization, string serviceId);
    public Task<Specialization?> GetSpecializationByIdAsync(string id);
    public Task EditSpecializationAsync(Specialization specialization, string specializationName, bool isActive, string serviceId);
    public Task<int> SaveChangesAsync();
}