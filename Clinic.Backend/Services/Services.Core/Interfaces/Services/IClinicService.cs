using Services.Core.Enums;
using Services.Core.Responses;

namespace Services.Core.Interfaces.Services;

public interface IClinicService
{
    public Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive);
    public Task AddSpecializationAsync(string specializationName, bool isActive, string serviceId);
    public Task EditSpecializationAsync(string id, string specializationName, bool isActive, string serviceId);
    public Task EditServiceAsync(string id, string serviceName, float price, bool isActive, Category category);
    public Task<List<GetServicesResponse>> GetServicesAsync(Category category);
    public Task<List<GetSpecializationsResponse>> GetSpecializationsAsync();
    public Task<GetServiceResponse> GetServiceAsync(string id);
    public Task<GetSpecializationResponse> GetSpecializationAsync(string id);
    public Task ChangeSpecializationStatusAsync(string id);
    public Task ChangeServiceStatusAsync(string id);
}