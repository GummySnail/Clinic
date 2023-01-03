using Offices.Core.Interfaces.Data.Repositories;
using Offices.Infrastructure;
using Offices.Infrastructure.Data.Repositories;
using Offices.Infrastructure.Mapping;
using Offices.Infrastructure.Models;

namespace Offices.Api.Configuration;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<OfficeDatabaseSettings>(config.GetSection("OfficeDatabase"));

        services.AddAutoMapper(typeof(MapperProfile).Assembly);
        
        services.AddSingleton<IOfficeRepository, OfficeRepository>();
        
        return services;
    }
}