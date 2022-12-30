using Profiles.Core.Entities;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IDoctorRepository
{
    public Task<int> CreateDoctorProfileAsync(Doctor doctor);
}