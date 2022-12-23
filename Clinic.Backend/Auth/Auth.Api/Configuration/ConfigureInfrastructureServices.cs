using Auth.Core.Entities;
using Auth.Core.Interface.Data.Repositories;
using Auth.Core.Interface.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Data.Repositories;
using Auth.Infrastructure.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Configuration;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AuthenticationDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("identitySqlConnection"),
                build => build.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName));
        });

        services.AddIdentity<Account, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

        var builder = services.AddIdentityServer(opt =>
            {
                opt.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = c =>
                    c.UseSqlServer(config.GetConnectionString("sqlConnection"),
                        sql => sql.MigrationsAssembly("Auth.Infrastructure"));
            })
            .AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = c =>
                    c.UseSqlServer(config.GetConnectionString("sqlConnection"),
                        sql => sql.MigrationsAssembly("Auth.Infrastructure"));
            })
            .AddAspNetIdentity<Account>();
        
            builder.AddDeveloperSigningCredential();
        
            services.AddAuthentication(opt =>
                {
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt => {
                    opt.AccessDeniedPath = "/Auth/AccessDenied";
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
                {
                    opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.Authority = "https://localhost:5005";
                    opt.ClientId = "client";
                    opt.ResponseType = OpenIdConnectResponseType.Code;
                    opt.SaveTokens = true;
                    opt.ClientSecret = "client-secret";
                    
                    opt.Scope.Add("openid");
                    opt.Scope.Add("offline_access");
                    opt.Scope.Add("api");
                    opt.Scope.Add("UserInfoScope");
                    opt.Scope.Add("user-profile");
                    opt.GetClaimsFromUserInfoEndpoint = true;
                    
                });
            /*.AddAspNetIdentity<Account>()
            .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
            .AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
            .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityServerConfiguration.Clients)
            .AddDeveloperSigningCredential();*/
            

        services.AddScoped<IEmailService, EmailService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}