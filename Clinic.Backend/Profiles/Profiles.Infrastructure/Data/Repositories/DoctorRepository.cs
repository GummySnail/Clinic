using AutoMapper;
using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Infrastructure.Data.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfileDbContext _context;
    private readonly IMapper _mapper;
    public DoctorRepository(ProfileDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateDoctorProfileAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
    }

    public Task<Doctor?> GetDoctorByIdAsync(string id)
    {
        var result = _context.Doctors.SingleOrDefault(x => x.Id == id);
        
        return Task.FromResult(result); 
    }

    public async Task<PagedList<Doctor>> GetDoctorsAtWorkAsync(SearchParams searchParams)
    {
        var query = _context.Doctors
            .Where(d => d.Status == Status.AtWork && 
                        (d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                         d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                         d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower())
                        ));

        query = searchParams.OrderByExperience switch
        {
            "Upcoming" => query.OrderBy(q => q.CareerStartYear),
            _ => query.OrderByDescending(q => q.CareerStartYear)
        };

        return await PagedList<Doctor>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);
    }

    public void ChangeDoctorStatusAsync(Doctor doctor, Status status)
    {
        doctor.Status = status;
    }

    public Task<DoctorProfileResponse> MappingToDoctorProfileResponse(Doctor doctor)
    {
        var result = _mapper.Map<Doctor, DoctorProfileResponse>(doctor);

        return Task.FromResult(result);
    }

    public Task<ICollection<DoctorProfileResponse>> MappingToCollectionDoctorProfileResponse(PagedList<Doctor> doctors)
    {
        List<DoctorProfileResponse> doctorsList = new List<DoctorProfileResponse>();

        foreach (var doctor in doctors)
        {
            doctorsList.Add(_mapper.Map<Doctor, DoctorProfileResponse>(doctor));
        }

        return Task.FromResult<ICollection<DoctorProfileResponse>>(doctorsList);
    }

    public Task<ICollection<DoctorProfileSearchByAdminResponse>> MappingToDoctorProfileSearchByAdminResponse(PagedList<Doctor> doctors)
    {
        List<DoctorProfileSearchByAdminResponse> doctorsList = new List<DoctorProfileSearchByAdminResponse>();

        foreach (var doctor in doctors)
        {
            doctorsList.Add(_mapper.Map<Doctor, DoctorProfileSearchByAdminResponse>(doctor));
        }

        return Task.FromResult<ICollection<DoctorProfileSearchByAdminResponse>>(doctorsList);
    }

    public async Task<PagedList<Doctor>> GetDoctorsByAdminAsync(SearchParams searchParams)
    {
        var query = _context.Doctors
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                         d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                         d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        return await PagedList<Doctor>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);
    }
}