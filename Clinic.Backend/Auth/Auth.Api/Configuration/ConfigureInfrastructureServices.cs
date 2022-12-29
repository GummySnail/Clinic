using System.IdentityModel.Tokens.Jwt;
using Auth.Core.Entities;
using Auth.Core.Interface.Data.Repositories;
using Auth.Core.Interface.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Data.Repositories;
using Auth.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Auth.Api.Configuration;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddDbContext<AuthenticationDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"),
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
            .AddAspNetIdentity<Account>()
            .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
            .AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
            .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityServerConfiguration.Clients);
            
        builder.AddDeveloperSigningCredential();

        services.ConfigureApplicationCookie(opt =>
        {
            opt.LoginPath = "/auth/signin";
            opt.LogoutPath = "/auth/logout";
        });
        
        services.AddAuthentication(opt =>
            {
                opt.DefaultChallengeScheme = "oidc"; 
            })
            .AddCookie("Cookie", opt =>
            {
                opt.Events.OnSigningOut = async e =>
                {
                    await e.HttpContext.RevokeUserRefreshTokenAsync();
                };
            })
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.Authority = "https://localhost:5005";
                opt.RequireHttpsMetadata = false;
                opt.ClientId = "client";
                opt.ClientSecret = "client-secret";
                opt.ResponseType = "code";
                opt.SaveTokens = true;
                
                opt.GetClaimsFromUserInfoEndpoint = true;
                
                opt.Scope.Clear();
                opt.Scope.Add("openid");
                opt.Scope.Add("offline_access");
                opt.Scope.Add("UserInfoScope");
                opt.Scope.Add("user-profile");
            });
        
        services.AddAccessTokenManagement(options =>
            {
                options.Client.DefaultClient.Scope = "user-profile";
            })
            .ConfigureBackchannelHttpClient();

        services.AddScoped<IEmailService, EmailService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}