using Profiles.Core.Entities;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IPatientRepository
{
    public Task<int> CreatePatientProfileAsync(Patient patient);
}