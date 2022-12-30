using Microsoft.EntityFrameworkCore;
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


    public async Task CreatePatientProfileAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    //I can better do this method but later =)
    public async Task<bool> IsProfileExistAsync(string firstName, string lastName, string? middleName, DateTime dateOfBirth)
    {
        var coeff = 0;
        var profiles = _context.Patients
            .Where(x => x.FirstName.ToLowerInvariant() == firstName.ToLowerInvariant() ||
                             x.LastName.ToLowerInvariant() == lastName.ToLowerInvariant() ||
                             x.MiddleName.ToLowerInvariant() == middleName.ToLowerInvariant() ||
                             x.DateOfBirth.Date == dateOfBirth.Date);

        foreach (var profile in profiles)
        {
            if (profile.FirstName.ToLowerInvariant() == firstName.ToLowerInvariant())
            {
                coeff += 5;
            }

            if (profile.LastName.ToLowerInvariant() == lastName.ToLowerInvariant())
            {
                coeff += 5;
            }

            if (profile.MiddleName?.ToLowerInvariant() == middleName?.ToLowerInvariant())
            {
                coeff += 5;
            }

            if (profile.DateOfBirth.Date == dateOfBirth.Date)
            {
                coeff += 3;
            }

            if (coeff >= 13)
            {
                return true;
                //return profile;
            }
        }

        return false;
    }
}