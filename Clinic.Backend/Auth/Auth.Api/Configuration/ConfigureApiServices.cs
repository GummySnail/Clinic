namespace Auth.Api.Configuration;

public static class ConfigureApiServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {   
        services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
        services.AddControllersWithViews();

        return services;
    }
}