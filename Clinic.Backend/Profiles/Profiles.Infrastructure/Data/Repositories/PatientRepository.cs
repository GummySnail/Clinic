using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profiles.Core.Entities;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Infrastructure.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ProfileDbContext _context;
    private readonly IMapper _mapper;

    public PatientRepository(ProfileDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task CreatePatientProfileAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    public async Task<Patient?> GetPatientByIdAsync(string id)
    {
        var patient = _context.Patients.SingleOrDefault(x => x.Id == id);
        
        return patient;
    }

    public async Task<PatientProfileByDoctorResponse> MappingToPatientProfileByDoctorResponse(Patient patient)
    {
        var result = _mapper.Map<Patient, PatientProfileByDoctorResponse>(patient);

        return result;
    }

    public async Task<PagedList<Patient>> GetPatientsByAdminAsync(SearchParams searchParams)
    {
        var query = _context.Patients
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        return await PagedList<Patient>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);
    }

    public async Task<ICollection<PatientsProfileSearchByAdminResponse>> MappingToPatientsProfileSearchByAdminResponse(PagedList<Patient> patients)
    {
        List<PatientsProfileSearchByAdminResponse> patientsList = new List<PatientsProfileSearchByAdminResponse>();

        foreach (var patient in patients)
        {
            patientsList.Add(_mapper.Map<Patient, PatientsProfileSearchByAdminResponse>(patient));
        }

        return patientsList;
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