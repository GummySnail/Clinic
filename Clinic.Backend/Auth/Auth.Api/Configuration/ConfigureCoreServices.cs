using Auth.Core.Logic.Auth;

namespace Auth.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();

        return services;
    }
}