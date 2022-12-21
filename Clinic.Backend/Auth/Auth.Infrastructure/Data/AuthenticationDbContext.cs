using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthenticationDbContext : IdentityDbContext<Account>
{
    public AuthenticationDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureTableNames(builder);
    }

    public static void ConfigureTableNames(ModelBuilder builder)
    {
        builder.Entity<IdentityUser>().ToTable("Accounts");
    }
}