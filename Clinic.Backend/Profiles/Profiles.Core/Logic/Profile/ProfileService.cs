using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;
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
            throw new Exception();
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
            throw new Exception();
        }
    }

    public async Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName)
    {
        var receptionist = new Receptionist(firstName, lastName, middleName);
        
        await _repositoryManager.ReceptionistRepository.CreateReceptionistProfileAsync(receptionist);
        
        var result = await _repositoryManager.UnitOfWork.SaveChangesAsync();
        
        if (result == 0)
        {
            throw new Exception();
        }
        
    }

    public async Task<ICollection<DoctorProfileResponse>> GetDoctorsAtWorkAsync(SearchParams searchParams)
    {
        var doctors = await _repositoryManager.DoctorRepository.GetDoctorsAtWorkAsync(searchParams);

        return _repositoryManager.DoctorRepository.MappingToDoctorProfileResponse(doctors);
    }

    public async Task<ICollection<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(SearchParams searchParams)
    {
        var doctors = await _repositoryManager.DoctorRepository.GetDoctorsByAdminAsync(searchParams);
        
        return _repositoryManager.DoctorRepository.MappingToDoctorProfileSearchByAdminResponse(doctors);
    }

    public async Task<ICollection<ReceptionistProfileResponse>> GetReceptionistsAsync(SearchParams searchParams)
    {
        var receptionists = await _repositoryManager.ReceptionistRepository.GetReceptionistsAsync(searchParams);

        return await _repositoryManager.ReceptionistRepository.MappingToReceptionistProfileResponse(receptionists);
    }
}