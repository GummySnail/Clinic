using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IProfileRepository
{
    public Task CreateDoctorProfileAsync(Doctor doctor);
    public Task<Doctor?> GetDoctorByIdAsync(string id);
    public Task<PagedList<Doctor>> GetDoctorsAtWorkAsync(SearchParams searchParams);
    public Task<PagedList<Doctor>> GetDoctorsByAdminAsync(SearchParams searchParams);
    public void ChangeDoctorStatusAsync(Doctor doctor, Status status);
    public Task<DoctorProfileResponse> MappingToDoctorProfileResponse(Doctor doctor);
    public Task<ICollection<DoctorProfileResponse>> MappingToCollectionDoctorProfileResponse(PagedList<Doctor> doctors);
    public Task<ICollection<DoctorProfileSearchByAdminResponse>> MappingToDoctorProfileSearchByAdminResponse(PagedList<Doctor> doctors);
    public Task CreateReceptionistProfileAsync(Receptionist receptionist);
    public Task<PagedList<Receptionist>> GetReceptionistsAsync(SearchParams searchParams);
    public Task<Receptionist?> GetReceptionistByIdAsync(string id);
    public void DeleteReceptionist(Receptionist receptionist);
    public Task<ReceptionistProfileByIdResponse> MappingToReceptionistProfileByIdResponse(Receptionist receptionist);
    public Task<ICollection<ReceptionistProfileResponse>> MappingToCollectionReceptionistProfileResponse(
        PagedList<Receptionist> receptionists);
    public Task CreatePatientProfileAsync(Patient patient);
    public Task<Patient?> GetPatientByIdAsync(string id);
    public Task<PatientProfileByDoctorResponse> MappingToPatientProfileByDoctorResponse(Patient patient);
    public Task<PatientProfileByAdminResponse> MappingToPatientProfileByAdminResponse(Patient patient);
    public Task<PagedList<Patient>> GetPatientsByAdminAsync(SearchParams searchParams);
    public Task<ICollection<PatientsProfileSearchByAdminResponse>> MappingToPatientsProfileSearchByAdminResponse(PagedList<Patient> patients);
    public void DeletePatient(Patient patient);
    public Task<int> SaveChangesAsync();
}