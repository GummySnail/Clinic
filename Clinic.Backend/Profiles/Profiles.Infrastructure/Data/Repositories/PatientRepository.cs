using Profiles.Core.Entities;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Infrastructure.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ProfileDbContext _context;

    public PatientRepository(ProfileDbContext context)
    {
        _context = context;
    }


    public async Task<int> CreatePatientProfileAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        return await _context.SaveChangesAsync();
    }
}