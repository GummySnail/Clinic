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

        var serviceSpecialization = new List<ServiceSpecialization> { new() { ServiceId = serviceId, SpecializationId = specialization.Id} };

        await _context.ServiceSpecializations.AddRangeAsync(serviceSpecialization);
    }

    public async Task<Specialization?> GetSpecializationByIdAsync(string id)
    {
        return await _context.Specializations.Include(x => x.Services).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task EditSpecializationAsync(Specialization specialization, string specializationName, bool isActive, string serviceId)
    {
        specialization.SpecializationName = specializationName;
        specialization.IsActive = isActive;

        var serviceSpecializationToDelete = await _context.ServiceSpecializations
            .Where(ss => ss.SpecializationId == specialization.Id)
            .ToListAsync();

        _context.ServiceSpecializations.RemoveRange(serviceSpecializationToDelete);

        var serviceSpecialization = new List<ServiceSpecialization> { new() { ServiceId = serviceId, SpecializationId = specialization.Id} };
        
        await _context.ServiceSpecializations.AddRangeAsync(serviceSpecialization);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}