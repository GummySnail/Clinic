using Auth.Core.Entities;
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
    
    public static async Task SetWorkers(UserManager<Account> userManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var doctors = new List<Account>
        {
            new()
            {
                UserName = "Alexander",
                Email = "alexanderdoctor@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Vitaut",
                Email = "vitautdoctor@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Zlata",
                Email = "zlatadoctor@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Kate",
                Email = "katedoctor@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Tadeush",
                Email = "tadeushdoctor@mail.ru",
                EmailConfirmed = true
            }
        };
        
        var receptionists = new List<Account>
        {
            new()
            {
                UserName = "Vlad",
                Email = "vladreceptionist@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Alesya",
                Email = "alesyareceptionist@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Darina",
                Email = "darinareceptionist@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Maxim",
                Email = "maximreceptionist@mail.ru",
                EmailConfirmed = true
            },
            new()
            {
                UserName = "Mihailo",
                Email = "mihailoreceptionist@mail.ru",
                EmailConfirmed = true
            }
        };
        
        foreach (var doctor in doctors)
        {
            await userManager.CreateAsync(doctor, "pa$$w0rd");
            await userManager.AddToRoleAsync(doctor, "Doctor");
        }

        foreach (var receptionist in receptionists)
        {
            await userManager.CreateAsync(receptionist,"pa$$w0rd");
            await userManager.AddToRoleAsync(receptionist, "Receptionist");
        }
    }
}