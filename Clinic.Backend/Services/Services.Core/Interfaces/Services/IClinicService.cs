using Services.Core.Enums;
using Services.Core.Responses;

namespace Services.Core.Interfaces.Services;

public interface IClinicService
{
    public Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive);
    public Task AddSpecializationAsync(string specializationName, bool isActive, string serviceId);
    public Task EditSpecializationAsync(string specializationId, string specializationName, bool isActive, string serviceId);
    public Task EditServiceAsync(string serviceId, string serviceName, float price, bool isActive, Category category);
    public Task<List<GetServicesResponse>> GetServicesAsync(Category category);
    public Task<List<GetSpecializationsResponse>> GetSpecializationsAsync();
    public Task<GetServiceResponse> GetServiceAsync(string serviceId);
    public Task<GetSpecializationResponse> GetSpecializationAsync(string specializationId);
    public Task ChangeSpecializationStatusAsync(string specializationId);
    public Task ChangeServiceStatusAsync(string serviceId);
}