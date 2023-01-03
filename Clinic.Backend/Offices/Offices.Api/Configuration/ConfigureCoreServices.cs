using Offices.Core.Logic;

namespace Offices.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<OfficeService>();
        return services;
    }
}