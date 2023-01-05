using Services.Core.Logic;

namespace Services.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<LogicService>();
        return services;
    }
}