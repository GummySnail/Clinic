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

    public async Task<PagedList<Doctor>> GetDoctorsAtWorkAsync(DoctorParams doctorParams)
    {
        var query = _context.Doctors
            .Where(d => d.Status == Status.AtWork && 
                        (d.FirstName.ToLower().Contains(doctorParams.FullName.ToLower()) ||
                         d.LastName.ToLower().Contains(doctorParams.FullName.ToLower()) ||
                         d.MiddleName.ToLower().Contains(doctorParams.FullName.ToLower())
                        ));

        query = doctorParams.OrderByExperience switch
        {
            "Upcoming" => query.OrderBy(q => q.CareerStartYear),
            _ => query.OrderByDescending(q => q.CareerStartYear)
        };

        return await PagedList<Doctor>
            .CreateAsync(query, doctorParams.PageNumber, doctorParams.PageSize);
    }

    public ICollection<DoctorProfileResponse> MappingToDoctorProfileResponse(PagedList<Doctor> doctors)
    {
        List<DoctorProfileResponse> doctorsList = new List<DoctorProfileResponse>();

        foreach (var doctor in doctors)
        {
            doctorsList.Add(_mapper.Map<Doctor, DoctorProfileResponse>(doctor));
        }

        return doctorsList;
    }

    public ICollection<DoctorProfileSearchByAdminResponse> MappingToDoctorProfileSearchByAdminResponse(PagedList<Doctor> doctors)
    {
        List<DoctorProfileSearchByAdminResponse> doctorsList = new List<DoctorProfileSearchByAdminResponse>();

        foreach (var doctor in doctors)
        {
            doctorsList.Add(_mapper.Map<Doctor, DoctorProfileSearchByAdminResponse>(doctor));
        }

        return doctorsList;
    }

    public async Task<PagedList<Doctor>> GetDoctorsByAdminAsync(DoctorParams doctorParams)
    {
        var query = _context.Doctors
            .Where(d => d.FirstName.ToLower().Contains(doctorParams.FullName.ToLower()) ||
                         d.LastName.ToLower().Contains(doctorParams.FullName.ToLower()) ||
                         d.MiddleName.ToLower().Contains(doctorParams.FullName.ToLower()));

        return await PagedList<Doctor>
            .CreateAsync(query, doctorParams.PageNumber, doctorParams.PageSize);
    }
}