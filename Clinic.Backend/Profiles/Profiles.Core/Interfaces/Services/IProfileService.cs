using Microsoft.AspNetCore.Http;
using Profiles.Core.Enums;
using Profiles.Core.Pagination;
using Profiles.Core.Responses;

namespace Profiles.Core.Interfaces.Services;

public interface IProfileService
{
    public Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName, DateTime dateOfBirth,
        string phoneNumber, IFormFile? profilePhoto);
    public Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status, IFormFile? profilePhoto);
    public Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName, IFormFile? profilePhoto);
    public Task CreatePatientProfileByAdminAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth);
    public Task<ICollection<DoctorProfileResponse>> GetDoctorsAtWorkAsync(SearchParams searchParams);
    public Task<ICollection<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(SearchParams searchParams);
    public Task<ICollection<ReceptionistProfileResponse>> GetReceptionistsAsync(SearchParams searchParams);
    public Task<ICollection<PatientsProfileSearchByAdminResponse>> GetPatientsByAdminAsync(SearchParams searchParams);
    public Task<PatientProfileByDoctorResponse> DoctorGetPatientProfileByIdAsync(string patientId);
    public Task<PatientProfileByAdminResponse> AdminGetPatientProfileByIdAsync(string patientId);
    public Task<DoctorProfileResponse> GetDoctorProfileByIdAsync(string doctorId);
    public Task<ReceptionistProfileByIdResponse> GetReceptionistProfileByIdAsync(string receptionistId);
    public Task DeletePatientProfileAsync(string patientId);
    public Task DeleteReceptionistProfileAsync(string receptionistId);
    public Task UpdateDoctorStatusAsync(string doctorId, Status status);
}