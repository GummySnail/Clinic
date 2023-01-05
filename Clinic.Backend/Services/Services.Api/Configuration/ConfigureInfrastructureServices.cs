using Microsoft.EntityFrameworkCore;
using Services.Core.Interfaces.Data.Repositories;
using Services.Infrastructure.Data;
using Services.Infrastructure.Data.Repositories;

namespace Services.Api.Configuration;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ServicesDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                build => build.MigrationsAssembly(typeof(ServicesDbContext).Assembly.FullName));
        });

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }

    public static async Task AddDatabaseConfiguration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ServicesDbContext>();
            await context.Database.MigrateAsync();
            await DataSeeder.SetServiceCategories(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error during database migration");
        }
    }
}