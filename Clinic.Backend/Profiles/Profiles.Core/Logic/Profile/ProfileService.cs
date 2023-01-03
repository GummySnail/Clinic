using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Core.Logic.Profile.Exceptions;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Logic.Profile;

public class ProfileService
{
    private readonly IRepositoryManager _repositoryManager;
    public ProfileService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, string phoneNumber)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth, phoneNumber);
        
        await _repositoryManager.PatientRepository.CreatePatientProfileAsync(patient);
        
        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        if (result == 0)
        {
            throw new DatabaseException("Unable to create patient");
        }
    }

    public async Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status)
    {
        var doctor = new Doctor(firstName, lastName, middleName, dateOfBirth, careerStartYear, status);
        
        await _repositoryManager.DoctorRepository.CreateDoctorProfileAsync(doctor);
        
        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        if (result == 0)
        {
            throw new DatabaseException("Unable to create profile");
        }
    }

    public async Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName)
    {
        var receptionist = new Receptionist(firstName, lastName, middleName);
        
        await _repositoryManager.ReceptionistRepository.CreateReceptionistProfileAsync(receptionist);
        
        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        if (result == 0)
        {
            throw new DatabaseException("Unable to create receptionist");
        }
        
    }

    public async Task CreatePatientProfileByAdminAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth);
        
        await _repositoryManager.PatientRepository.CreatePatientProfileAsync(patient);
        
        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        if (result == 0)
        {
            throw new DatabaseException("Unable to create patient");
        }
    }

    public async Task<ICollection<DoctorProfileResponse>> GetDoctorsAtWorkAsync(SearchParams searchParams)
    {
        var doctors = await _repositoryManager.DoctorRepository.GetDoctorsAtWorkAsync(searchParams);

        var result = await _repositoryManager.DoctorRepository.MappingToCollectionDoctorProfileResponse(doctors);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }

    public async Task<ICollection<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(SearchParams searchParams)
    {
        var doctors = await _repositoryManager.DoctorRepository.GetDoctorsByAdminAsync(searchParams);
        
        var result = await _repositoryManager.DoctorRepository.MappingToDoctorProfileSearchByAdminResponse(doctors);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }

    public async Task<ICollection<ReceptionistProfileResponse>> GetReceptionistsAsync(SearchParams searchParams)
    {
        var receptionists = await _repositoryManager.ReceptionistRepository.GetReceptionistsAsync(searchParams);

        var result = await _repositoryManager.ReceptionistRepository.MappingToCollectionReceptionistProfileResponse(receptionists);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }

    public async Task<ICollection<PatientsProfileSearchByAdminResponse>> GetPatientsByAdminAsync(SearchParams searchParams)
    {
        var patients = await _repositoryManager.PatientRepository.GetPatientsByAdminAsync(searchParams);

        var result = await _repositoryManager.PatientRepository.MappingToPatientsProfileSearchByAdminResponse(patients);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }

    public async Task<PatientProfileByDoctorResponse> DoctorGetPatientProfileByIdAsync(string id)
    {
        var patient = await _repositoryManager.PatientRepository.GetPatientByIdAsync(id);
        
        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }

        var result = await _repositoryManager.PatientRepository.MappingToPatientProfileByDoctorResponse(patient);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }

    public async Task<PatientProfileByAdminResponse> AdminGetPatientProfileByIdAsync(string id)
    {
        var patient = await _repositoryManager.PatientRepository.GetPatientByIdAsync(id);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }

        var result = await _repositoryManager.PatientRepository.MappingToPatientProfileByAdminResponse(patient);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }
    
    public async Task<DoctorProfileResponse> GetDoctorProfileByIdAsync(string id)
    {
        var doctor = await _repositoryManager.DoctorRepository.GetDoctorByIdAsync(id);

        if (doctor is null)
        {
            throw new NotFoundException("Doctor is not exist");
        }

        var result = await _repositoryManager.DoctorRepository.MappingToDoctorProfileResponse(doctor);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }
        
        return result;
    }

    public async Task<ReceptionistProfileByIdResponse> GetReceptionistProfileByIdAsync(string id)
    {
        var receptionist = await _repositoryManager.ReceptionistRepository.GetReceptionistByIdAsync(id);

        if (receptionist is null)
        {
            throw new NotFoundException("Receptionist is not exist");
        }

        var result = await _repositoryManager.ReceptionistRepository.MappingToReceptionistProfileByIdResponse(receptionist);

        if (result is null)
        {
            throw new Exception("Error during mapping");
        }

        return result;
    }

    public async Task DeletePatientProfileAsync(string id)
    {
        var patient = await _repositoryManager.PatientRepository.GetPatientByIdAsync(id);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }

        _repositoryManager.PatientRepository.DeletePatientAsync(patient);
        
        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        if (result == 0)
        {
            throw new DatabaseException("Unable to delete patient profile");
        }
    }
}