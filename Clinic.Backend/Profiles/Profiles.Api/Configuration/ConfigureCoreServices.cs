using Profiles.Core.Logic.Profile;

namespace Profiles.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<ProfileService>();
        return services;
    }
}