using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthDbContext : IdentityDbContext<Account>
{
    public AuthDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureTableNames(builder);
    }

    public static void ConfigureTableNames(ModelBuilder builder)
    {
        builder.Entity<Account>().ToTable("Accounts");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AccountClaims");
        builder.Entity<IdentityUserRole<string>>().ToTable("AccountRoles");
        builder.Entity<IdentityUserLogin<string>>().ToTable("AccountLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("AccountTokens");
    }
}