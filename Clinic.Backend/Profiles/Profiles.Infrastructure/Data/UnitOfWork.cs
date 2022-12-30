using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfileDbContext _context;

    public UnitOfWork(ProfileDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
}