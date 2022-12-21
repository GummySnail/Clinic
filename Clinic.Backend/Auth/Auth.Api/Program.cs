using Auth.Api.Configuration;
using Auth.Core.Interface.Data.Repositories;
using Auth.Core.Logic.Auth;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Data.Repositories;
using Auth.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthenticationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        build => build.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName));
});

builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequiredLength = 6;
        opt.Password.RequireUppercase = false;
        opt.User.RequireUniqueEmail = true;
        //opt.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(opt =>
    {
        opt.EmitStaticAudienceClaim = true;
    })
    .AddAspNetIdentity<User>()
    .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
    .AddInMemoryIdentityResources(IdentityServerConfiguration.IdentityResources)
    .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityServerConfiguration.Clients)
    .AddDeveloperSigningCredential();

/*builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "";
    opt.LogoutPath = "";
});*/

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AuthenticationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await context.Database.MigrateAsync();
    await DataSeeder.SetApplicationRoleConfiguration(roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error.DatabaseMigration");
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseIdentityServer();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();