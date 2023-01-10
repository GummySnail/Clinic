using Services.Core.Enums;

namespace Services.Core.Interfaces.Services;

public interface IClinicService
{
    public Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive);
    public Task AddSpecializationAsync(string specializationName, bool isActive, string serviceId);
    public Task EditSpecializationAsync(string id, string specializationName, bool isActive, string serviceId);
}