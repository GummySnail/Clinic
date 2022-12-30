using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Core.Logic.Profile;

public class ProfileService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;

    public ProfileService(IPatientRepository patientRepository, IDoctorRepository doctorRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
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
}