using Microsoft.EntityFrameworkCore;
using Services.Core.Entities;

namespace Services.Infrastructure.Data;

public class ServicesDbContext : DbContext
{
    public ServicesDbContext(DbContextOptions<ServicesDbContext> options) : base(options) { }

    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<ServiceSpecialization> ServiceSpecializations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ServiceSpecialization>()
            .HasKey(ss => new { ss.ServiceId, ss.SpecializationId });

        builder.Entity<ServiceSpecialization>()
            .HasOne(ss => ss.Service)
            .WithMany(s => s.Specializations)
            .HasForeignKey(ss => ss.ServiceId);

        builder.Entity<ServiceSpecialization>()
            .HasOne(ss => ss.Specialization)
            .WithMany(s => s.Services)
            .HasForeignKey(ss => ss.SpecializationId);
    }
}