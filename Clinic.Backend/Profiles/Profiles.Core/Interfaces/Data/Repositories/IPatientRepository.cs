using Profiles.Core.Entities;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IPatientRepository
{
    public Task CreatePatientProfileAsync(Patient patient);
    public Task<Patient?> GetPatientByIdAsync(string id);
    public Task<PatientProfileByDoctorResponse> MappingToPatientProfileByDoctorResponse(Patient patient);
    public Task<PagedList<Patient>> GetPatientsByAdminAsync(SearchParams searchParams);
    public Task<ICollection<PatientsProfileSearchByAdminResponse>> MappingToPatientsProfileSearchByAdminResponse(PagedList<Patient> patients);
    public Task<bool> IsProfileExistAsync(string firstName, string lastName, string? middleName, DateTime dateOfBirth);
}