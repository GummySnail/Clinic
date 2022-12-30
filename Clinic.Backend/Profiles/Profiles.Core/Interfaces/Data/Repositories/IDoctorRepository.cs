using Profiles.Core.Entities;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IDoctorRepository
{
    public Task CreateDoctorProfileAsync(Doctor doctor);
    public Task<PagedList<Doctor>> GetDoctorsAtWorkAsync(DoctorParams doctorParams);
    public Task<PagedList<Doctor>> GetDoctorsByAdminAsync(DoctorParams doctorParams);
    public ICollection<DoctorProfileResponse> MappingToDoctorProfileResponse(PagedList<Doctor> doctors);
    public ICollection<DoctorProfileSearchByAdminResponse> MappingToDoctorProfileSearchByAdminResponse(PagedList<Doctor> doctors);
}