﻿using Microsoft.EntityFrameworkCore;
using Services.Core.Entities;
using Services.Core.Enums;
using Services.Core.Exceptions;
using Services.Core.Interfaces.Services;
using Services.Infrastructure.Data;

namespace Services.Infrastructure.Services;

public class ClinicService : IClinicService
{
    private readonly ServicesDbContext _context;

    public ClinicService(ServicesDbContext context)
    {
        _context = context;
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
}