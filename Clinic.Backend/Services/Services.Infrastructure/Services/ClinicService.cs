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
    
    public async Task EditSpecializationAsync(string specializationId, string specializationName, bool isActive, string serviceId)
    {
        var specialization = await _context.Specializations
            .SingleOrDefaultAsync(x => x.Id == specializationId);

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

    public async Task EditServiceAsync(string serviceId, string serviceName, float price, bool isActive, Category serviceCategory)
    {
        var category = await _context.ServiceCategories
            .SingleOrDefaultAsync(x => x.CategoryName == serviceCategory);

        if (category is null)
        {
            throw new NotFoundException("Category is not exist");
        }
        
        var service = await _context.Services.SingleOrDefaultAsync(x => x.Id == serviceId);

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

    public async Task<List<GetServicesResponse>> GetServicesAsync(Category category)
    {
        var servicesCategory = await _context.ServiceCategories.AsNoTracking().SingleOrDefaultAsync(x => x.CategoryName == category);
        var services = await _context.Services.AsNoTracking().Where(x => x.CategoryId == servicesCategory.Id).ToListAsync();
        
        List<GetServicesResponse> servicesList = new List<GetServicesResponse>();

        foreach (var service in services)
        {
            servicesList.Add(_mapper.Map<Service, GetServicesResponse>(service));
        }

        return servicesList;
    }

    public async Task<List<GetSpecializationsResponse>> GetSpecializationsAsync()
    {
        var specializations = await _context.Specializations.AsNoTracking().ToListAsync();

        List<GetSpecializationsResponse> specializationsList = new List<GetSpecializationsResponse>();

        foreach (var specialization in specializations)
        {
            specializationsList.Add(_mapper.Map<Specialization, GetSpecializationsResponse>(specialization));
        }
        
        return specializationsList;
    }

    public async Task<GetServiceResponse> GetServiceAsync(string serviceId)
    {
        var service = await _context.Services.AsNoTracking().Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == serviceId);

        if (service is null)
        {
            throw new NotFoundException("Service is not exist");
        }

        var result = _mapper.Map<Service, GetServiceResponse>(service);

        return result;
    }

    public async Task<GetSpecializationResponse> GetSpecializationAsync(string specializationId)
    {
        var specialization = await _context.Specializations.Include(x => x.Services).ThenInclude(x => x.Service)
            .ThenInclude(x => x.Category).SingleOrDefaultAsync(x => x.Id == specializationId);

        if (specialization is null)
        {
            throw new NotFoundException("Specialization iss not exist");
        }
        
        var result = new GetSpecializationResponse(specialization.SpecializationName,
            specialization.Services.Select(x => x.Service.Price),
            specialization.Services.Select(x => x.Service.IsActive),
            specialization.Services.Select(x => x.Service.Category.CategoryName));

        return result;
    }

    public async Task ChangeSpecializationStatusAsync(string specializationId)
    {
        var specialization = await _context.Specializations.SingleOrDefaultAsync(x => x.Id == specializationId);

        if (specialization is null)
        {
            throw new NotFoundException("Specialization is not exist");
        }

        specialization.IsActive = !specialization.IsActive;

        await _context.SaveChangesAsync();
    }

    public async Task ChangeServiceStatusAsync(string serviceId)
    {
        var service = await _context.Services.SingleOrDefaultAsync(x => x.Id == serviceId);
        
        if (service is null)
        {
            throw new NotFoundException("Service is not exist");
        }

        service.IsActive = !service.IsActive;

        await _context.SaveChangesAsync();
    }
}