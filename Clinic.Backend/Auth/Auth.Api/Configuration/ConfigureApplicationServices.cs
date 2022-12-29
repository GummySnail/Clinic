using Auth.Core.Entities;
using Auth.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Configuration;

public static class ConfigureApplicationServices
{
    public static WebApplication AddApplicationConfiguration(this WebApplication app)
    {
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
        
        return app;
    }

    public static async Task AddDatabaseConfiguration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AuthenticationDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<Account>>();

            await context.Database.MigrateAsync();
            await DataSeeder.SetApplicationRoleConfiguration(roleManager);
            await DataSeeder.SetWorkers(userManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error.DatabaseMigration");
        }
    }
}