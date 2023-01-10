using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Exceptions;
using Services.Core.Interfaces.Services;
using Services.Core.Responses;
using Services.Infrastructure.Data;

namespace Services.Infrastructure.Services;

public class ClinicService : IClinicService
{
    private readonly ServicesDbContext _context;
    private readonly IMapper _mapper;
    public ClinicService(ServicesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddServiceAsync(string serviceName, float price, Category serviceCategory, bool isActive)
    {
        var category = await _context.ServiceCategories
            .SingleOrDefaultAsync(x => x.CategoryName == serviceCategory);

        if (category is null)
        {
            throw new NotFoundException("Category is not exist");
        }

        var service = new Service(serviceName, price, category.Id, isActive);
        
        await _context.Services.AddAsync(service);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task AddSpecializationAsync(string specializationName, bool isActive, string serviceId)
    {
        var specialization = new Specialization(specializationName, isActive);

        await _context.Specializations.AddAsync(specialization);

        var serviceSpecialization = new List<ServiceSpecialization> { new() { ServiceId = serviceId, SpecializationId = specialization.Id} };

        await _context.ServiceSpecializations.AddRangeAsync(serviceSpecialization);

        await _context.SaveChangesAsync();
    }
    
    public async Task EditSpecializationAsync(string id, string specializationName, bool isActive, string serviceId)
    {
        var specialization = await _context.Specializations
            .SingleOrDefaultAsync(x => x.Id == id);

        if (specialization is null)
        {
            throw new NotFoundException("Specialization is not exist");
        }
        
        specialization.SpecializationName = specializationName;
        specialization.IsActive = isActive;

        var serviceSpecializationToDelete = await _context.ServiceSpecializations
            .Where(ss => ss.SpecializationId == specialization.Id)
            .ToListAsync();

        _context.ServiceSpecializations.RemoveRange(serviceSpecializationToDelete);

        var serviceSpecialization = new List<ServiceSpecialization> { new() { ServiceId = serviceId, SpecializationId = specialization.Id} };
        
        await _context.ServiceSpecializations.AddRangeAsync(serviceSpecialization);

        await _context.SaveChangesAsync();
    }

    public async Task EditServiceAsync(string id, string serviceName, float price, bool isActive, Category serviceCategory)
    {
        var category = await _context.ServiceCategories
            .SingleOrDefaultAsync(x => x.CategoryName == serviceCategory);

        if (category is null)
        {
            throw new NotFoundException("Category is not exist");
        }
        
        var service = await _context.Services.SingleOrDefaultAsync(x => x.Id == id);

        if (service is null)
        {
            throw new NotFoundException("Service is not exist");
        }

        service.ServiceName = serviceName;
        service.Price = price;
        service.IsActive = isActive;
        service.CategoryId = category.Id;

        await _context.SaveChangesAsync();
    }

    public async Task<List<GetServicesByCategoryResponse>> GetServicesAsync(Category category)
    {
        var servicesCategory = await _context.ServiceCategories.AsNoTracking().SingleOrDefaultAsync(x => x.CategoryName == category);
        var services = await _context.Services.AsNoTracking().Where(x => x.CategoryId == servicesCategory.Id).ToListAsync();
        
        List<GetServicesByCategoryResponse> servicesList = new List<GetServicesByCategoryResponse>();

        foreach (var service in services)
        {
            servicesList.Add(_mapper.Map<Service, GetServicesByCategoryResponse>(service));
        }

        return servicesList;
    }

    public async Task<List<GetSpecializationsListResponse>> GetSpecializationsAsync()
    {
        var specializations = await _context.Specializations.AsNoTracking().ToListAsync();

        List<GetSpecializationsListResponse> specializationsList = new List<GetSpecializationsListResponse>();

        foreach (var specialization in specializations)
        {
            specializationsList.Add(_mapper.Map<Specialization, GetSpecializationsListResponse>(specialization));
        }
        
        return specializationsList;
    }

    public async Task ChangeSpecializationStatusAsync(string id)
    {
        var specialization = await _context.Specializations.SingleOrDefaultAsync(x => x.Id == id);

        if (specialization is null)
        {
            throw new NotFoundException("Specialization is not exist");
        }

        specialization.IsActive = !specialization.IsActive;

        await _context.SaveChangesAsync();
    }

    public async Task ChangeServiceStatusAsync(string id)
    {
        var service = await _context.Services.SingleOrDefaultAsync(x => x.Id == id);
        
        if (service is null)
        {
            throw new NotFoundException("Service is not exist");
        }

        service.IsActive = !service.IsActive;

        await _context.SaveChangesAsync();
    }
}