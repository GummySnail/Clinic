using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IDoctorRepository
{
    public Task CreateDoctorProfileAsync(Doctor doctor);
    public Task<Doctor?> GetDoctorByIdAsync(string id);
    public Task<PagedList<Doctor>> GetDoctorsAtWorkAsync(SearchParams searchParams);
    public Task<PagedList<Doctor>> GetDoctorsByAdminAsync(SearchParams searchParams);
    public void ChangeDoctorStatusAsync(Doctor doctor, Status status);
    public Task<DoctorProfileResponse> MappingToDoctorProfileResponse(Doctor doctor);
    public Task<ICollection<DoctorProfileResponse>> MappingToCollectionDoctorProfileResponse(PagedList<Doctor> doctors);
    public Task<ICollection<DoctorProfileSearchByAdminResponse>> MappingToDoctorProfileSearchByAdminResponse(PagedList<Doctor> doctors);
}