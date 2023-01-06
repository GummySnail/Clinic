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

    public async Task<ServiceCategory?> GetServiceCategoryAsync(Category serviceCategory)
    {
        var category = await _context.ServiceCategories.SingleOrDefaultAsync(x => x.CategoryName == serviceCategory);
        
        return category;
    }

    public async Task AddSpecializationAsync(Specialization specialization, string serviceId)
    {
        await _context.Specializations.AddAsync(specialization);

        var serviceList = new List<ServiceSpecialization> { new() { ServiceId = serviceId, SpecializationId = specialization.Id} };

        await _context.ServiceSpecializations.AddRangeAsync(serviceList);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}