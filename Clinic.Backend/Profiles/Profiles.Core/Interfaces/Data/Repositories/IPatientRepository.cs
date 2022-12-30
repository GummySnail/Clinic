using Profiles.Core.Entities;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IPatientRepository
{
    public Task<int> CreatePatientProfileAsync(Patient patient);
    public Task<bool> IsProfileExistAsync(string firstName, string lastName, string? middleName, DateTime dateOfBirth);
}