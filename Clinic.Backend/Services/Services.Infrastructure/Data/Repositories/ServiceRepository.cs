using Microsoft.EntityFrameworkCore;
using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Interfaces.Data.Repositories;

namespace Services.Infrastructure.Data.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly ServicesDbContext _context;

    public ServiceRepository(ServicesDbContext context)
    {
        _context = context;
    }

    public async Task AddServiceAsync(Service service)
    {
        await _context.Services.AddAsync(service);
    }
}