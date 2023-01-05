using Microsoft.EntityFrameworkCore;
using Services.Core.Entities;
using Services.Core.Enums;

namespace Services.Infrastructure.Data;

public class DataSeeder
{
    public static async Task SetServiceCategories(ServicesDbContext context)
    {
        if (await context.ServiceCategories.AnyAsync()) return;

        var categories = new List<ServiceCategory>
        {
            new() { CategoryName = Category.Analyses, TimeSlotSize = "15"},
            new() { CategoryName = Category.Consultation, TimeSlotSize = "30"},
            new() { CategoryName = Category.Diagnostics, TimeSlotSize = "60"}
        };

        foreach (var category in categories)
        {
            await context.ServiceCategories.AddAsync(category);
        }

        await context.SaveChangesAsync();
    }
}