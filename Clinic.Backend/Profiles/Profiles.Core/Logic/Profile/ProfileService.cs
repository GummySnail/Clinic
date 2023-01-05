using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Logic.Profile;

public class ProfileService
{
    private readonly IProfileRepository _profileRepository;
    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, string phoneNumber)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth, phoneNumber);
        
        await _profileRepository.CreatePatientProfileAsync(patient);
        
        await _profileRepository.SaveChangesAsync();
    }

    public async Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status)
    {
        var doctor = new Doctor(firstName, lastName, middleName, dateOfBirth, careerStartYear, status);
        
        await _profileRepository.CreateDoctorProfileAsync(doctor);
        
        await _profileRepository.SaveChangesAsync();
    }

    public async Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName)
    {
        var receptionist = new Receptionist(firstName, lastName, middleName);
        
        await _profileRepository.CreateReceptionistProfileAsync(receptionist);
        
        await _profileRepository.SaveChangesAsync();
    }

    public async Task CreatePatientProfileByAdminAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth);
        
        await _profileRepository.CreatePatientProfileAsync(patient);
        
        await _profileRepository.SaveChangesAsync();
    }

    public async Task<ICollection<DoctorProfileResponse>> GetDoctorsAtWorkAsync(SearchParams searchParams)
    {
        var doctors = await _profileRepository.GetDoctorsAtWorkAsync(searchParams);

        var result = await _profileRepository.MappingToCollectionDoctorProfileResponse(doctors);

        return result;
    }

    public async Task<ICollection<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(SearchParams searchParams)
    {
        var doctors = await _profileRepository.GetDoctorsByAdminAsync(searchParams);
        
        var result = await _profileRepository.MappingToDoctorProfileSearchByAdminResponse(doctors);

        return result;
    }

    public async Task<ICollection<ReceptionistProfileResponse>> GetReceptionistsAsync(SearchParams searchParams)
    {
        var receptionists = await _profileRepository.GetReceptionistsAsync(searchParams);

        var result = await _profileRepository.MappingToCollectionReceptionistProfileResponse(receptionists);

        return result;
    }

    public async Task<ICollection<PatientsProfileSearchByAdminResponse>> GetPatientsByAdminAsync(SearchParams searchParams)
    {
        var patients = await _profileRepository.GetPatientsByAdminAsync(searchParams);

        var result = await _profileRepository.MappingToPatientsProfileSearchByAdminResponse(patients);

        return result;
    }

    public async Task<PatientProfileByDoctorResponse> DoctorGetPatientProfileByIdAsync(string id)
    {
        var patient = await _profileRepository.GetPatientByIdAsync(id);

        var result = await _profileRepository.MappingToPatientProfileByDoctorResponse(patient);

        return result;
    }

    public async Task<PatientProfileByAdminResponse> AdminGetPatientProfileByIdAsync(string id)
    {
        var patient = await _profileRepository.GetPatientByIdAsync(id);

        var result = await _profileRepository.MappingToPatientProfileByAdminResponse(patient);

        return result;
    }
    
    public async Task<DoctorProfileResponse> GetDoctorProfileByIdAsync(string id)
    {
        var doctor = await _profileRepository.GetDoctorByIdAsync(id);

        var result = await _profileRepository.MappingToDoctorProfileResponse(doctor);

        return result;
    }

    public async Task<ReceptionistProfileByIdResponse> GetReceptionistProfileByIdAsync(string id)
    {
        var receptionist = await _profileRepository.GetReceptionistByIdAsync(id);

        var result = await _profileRepository.MappingToReceptionistProfileByIdResponse(receptionist);

        return result;
    }

    public async Task DeletePatientProfileAsync(string id)
    {
        var patient = await _profileRepository.GetPatientByIdAsync(id);

        _profileRepository.DeletePatient(patient);
        
        await _profileRepository.SaveChangesAsync();
    }

    public async Task DeleteReceptionistProfileAsync(string id)
    {
        var receptionist = await _profileRepository.GetReceptionistByIdAsync(id);

        _profileRepository.DeleteReceptionist(receptionist);

        await _profileRepository.SaveChangesAsync();
    }

    public async Task ChangeDoctorStatusAsync(string id, Status status)
    {
        var doctor = await _profileRepository.GetDoctorByIdAsync(id);

        _profileRepository.ChangeDoctorStatusAsync(doctor, status);
        
        await _profileRepository.SaveChangesAsync();
    }
}