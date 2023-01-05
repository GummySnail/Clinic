using Microsoft.EntityFrameworkCore;
using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Interfaces.Data.Repositories;

namespace Services.Infrastructure.Data.Repositories;

public class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly ServicesDbContext _context;

    public ServiceCategoryRepository(ServicesDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceCategory?> GetServiceCategoryAsync(Category serviceCategory)
    {
        var category = await _context.ServiceCategories.SingleOrDefaultAsync(x => x.CategoryName == serviceCategory);
        
        return category;
    }
}