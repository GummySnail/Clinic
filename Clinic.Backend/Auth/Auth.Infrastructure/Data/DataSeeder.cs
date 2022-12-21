using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class DataSeeder
{
    public static async Task SetApplicationRoleConfiguration(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<IdentityRole>
        {
            new() { Name = "Patient" },
            new() { Name = "Receptionist" },
            new() { Name = "Doctor" }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }
}