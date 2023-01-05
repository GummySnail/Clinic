using Services.Core.Entities;

namespace Services.Core.Interfaces.Data.Repositories;

public interface IServiceRepository
{
    public Task AddServiceAsync(Service service);
}