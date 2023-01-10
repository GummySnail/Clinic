using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Responses;

namespace Services.Core.Interfaces.Services;

public interface IClinicService
{
    public Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive);
    public Task AddSpecializationAsync(string specializationName, bool isActive, string serviceId);
    public Task EditSpecializationAsync(string id, string specializationName, bool isActive, string serviceId);
    public Task<List<GetServicesByCategoryResponse>> GetServicesAsync(Category category);
    public Task<List<GetSpecializationsListResponse>> GetSpecializationsAsync();
    public Task ChangeSpecializationStatusAsync(string id, bool isActive);
}