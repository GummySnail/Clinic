using Profiles.Core.Entities;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Infrastructure.Data.Repositories;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfileDbContext _context;

    public ReceptionistRepository(ProfileDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateReceptionistProfileAsync(Receptionist receptionist)
    {
        await _context.Receptionists.AddAsync(receptionist);
        return await _context.SaveChangesAsync();
    }
}