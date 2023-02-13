using System.IdentityModel.Tokens.Jwt;
using Auth.Api.Configuration;
using Auth.Core.Entities;
using Auth.Core.Interface.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var services = builder.Services;

//-- Infrastructure
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

var identityServerBuilder = services.AddIdentityServer(opt =>
    {
        opt.EmitStaticAudienceClaim = true;
    })
    .AddAspNetIdentity<Account>()
    .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources(config))
    .AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
    .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityServerConfiguration.Clients(config));
    
identityServerBuilder.AddDeveloperSigningCredential();

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
        opt.ClientSecret = config["ClientSecret"];
        opt.ResponseType = "code";
        opt.SaveTokens = true;
        
        opt.GetClaimsFromUserInfoEndpoint = true;
        
        opt.Scope.Clear();
        opt.Scope.Add("openid");
        opt.Scope.Add("offline_access");
        //opt.Scope.Add("ApiScope");
        opt.Scope.Add("UserInfoScope");
        opt.Scope.Add("user-profile");
    });

services.AddAccessTokenManagement(options =>
    {
        options.Client.DefaultClient.Scope = "user-profile";
    })
    .ConfigureBackchannelHttpClient();

services.AddScoped<IEmailService, EmailService>();

services.AddScoped<IAuthService, AuthService>();

//-- Api

services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
services.AddControllersWithViews();

var app = builder.Build();

//-- Middlewares 

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
        
app.UseAuthentication();

app.UseAuthorization();

app.UseCors(opt => opt
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
        
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

try
{
    var context = serviceProvider.GetRequiredService<AuthenticationDbContext>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();

    await context.Database.MigrateAsync();
    await DataSeeder.SetApplicationRoleConfiguration(roleManager);
    await DataSeeder.SetWorkers(userManager);
}
catch (Exception ex)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error.DatabaseMigration");
}

await app.RunAsync();