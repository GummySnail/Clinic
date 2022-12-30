using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Core.Logic.Profile;

public class ProfileService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IReceptionistRepository _receptionistRepository;
    public ProfileService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, IReceptionistRepository receptionistRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _receptionistRepository = receptionistRepository;
    }

    public async Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, string phoneNumber)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth, phoneNumber);
        var result = await _patientRepository.CreatePatientProfileAsync(patient);
        if (result == 0)
        {
            throw new Exception();
        }
    }

    public async Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status)
    {
        var doctor = new Doctor(firstName, lastName, middleName, dateOfBirth, careerStartYear, status);
        var result = await _doctorRepository.CreateDoctorProfileAsync(doctor);
        if (result == 0)
        {
            throw new Exception();
        }
    }

    public async Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName)
    {
        var receptionist = new Receptionist(firstName, lastName, middleName);
        var result = await _receptionistRepository.CreateReceptionistProfileAsync(receptionist);
        if (result == 0)
        {
            throw new Exception();
        }
    }
}