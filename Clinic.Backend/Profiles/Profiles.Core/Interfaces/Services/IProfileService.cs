using Profiles.Core.Enums;
using Profiles.Core.Pagination;
using Profiles.Core.Responses;

namespace Profiles.Core.Interfaces.Services;

public interface IProfileService
{
    public Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName, DateTime dateOfBirth,
        string phoneNumber);
    public Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status);
    public Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName);
    public Task CreatePatientProfileByAdminAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth);
    public Task<ICollection<DoctorProfileResponse>> GetDoctorsAtWorkAsync(SearchParams searchParams);
    public Task<ICollection<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(SearchParams searchParams);
    public Task<ICollection<ReceptionistProfileResponse>> GetReceptionistsAsync(SearchParams searchParams);
    public Task<ICollection<PatientsProfileSearchByAdminResponse>> GetPatientsByAdminAsync(SearchParams searchParams);
    public Task<PatientProfileByDoctorResponse> DoctorGetPatientProfileByIdAsync(string id);
    public Task<PatientProfileByAdminResponse> AdminGetPatientProfileByIdAsync(string id);
    public Task<DoctorProfileResponse> GetDoctorProfileByIdAsync(string id);
    public Task<ReceptionistProfileByIdResponse> GetReceptionistProfileByIdAsync(string id);
    public Task DeletePatientProfileAsync(string id);
    public Task DeleteReceptionistProfileAsync(string id);
    public Task ChangeDoctorStatusAsync(string id, Status status);
}