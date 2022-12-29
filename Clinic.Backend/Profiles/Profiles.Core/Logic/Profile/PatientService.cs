using Profiles.Core.Entities;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Core.Logic.Profile;

public class PatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth);
        var result = await _patientRepository.CreatePatientProfileAsync(patient);
        if (result == 0)
        {
            throw new Exception();
        }
    }
}