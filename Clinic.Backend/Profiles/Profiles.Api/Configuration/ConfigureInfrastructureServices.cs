using Microsoft.EntityFrameworkCore;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Infrastructure.Data;
using Profiles.Infrastructure.Data.Repositories;
using Profiles.Infrastructure.Mapping;

namespace Profiles.Api.Configuration;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ProfileDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                build => build.MigrationsAssembly(typeof(ProfileDbContext).Assembly.FullName));
        });

        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }

    public static async Task AddDatabaseConfiguration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ProfileDbContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error during database migration");
        }
    }
}