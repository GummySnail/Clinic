using Profiles.Core.Entities;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Infrastructure.Data.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfileDbContext _context;

    public DoctorRepository(ProfileDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateDoctorProfileAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        return await _context.SaveChangesAsync();
    }
}