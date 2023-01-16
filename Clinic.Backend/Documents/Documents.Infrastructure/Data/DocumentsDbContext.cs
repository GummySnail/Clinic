using Documents.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.Data;

public class DocumentsDbContext : DbContext
{
    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options) { }
    
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}