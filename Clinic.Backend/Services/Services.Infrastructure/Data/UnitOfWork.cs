using Services.Core.Interfaces.Data.Repositories;

namespace Services.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ServicesDbContext _context;

    public UnitOfWork(ServicesDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}