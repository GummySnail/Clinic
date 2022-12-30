using Profiles.Core.Entities;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IDoctorRepository
{
    public Task CreateDoctorProfileAsync(Doctor doctor);
    public Task<PagedList<Doctor>> GetDoctorsAtWorkAsync(DoctorParams doctorParams);
    public ICollection<DoctorProfileResponse> MappingToResponseListDoctorModel(PagedList<Doctor> doctors);
}