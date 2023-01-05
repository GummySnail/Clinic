using AutoMapper;
using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Infrastructure.Data.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ProfileDbContext _context;
    private readonly IMapper _mapper;

    public ProfileRepository(IMapper mapper, ProfileDbContext context)
    {
        _mapper = mapper;
        _context = context;
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
    public async Task CreateReceptionistProfileAsync(Receptionist receptionist)
    {
        await _context.Receptionists.AddAsync(receptionist);
    }

    public async Task<PagedList<Receptionist>> GetReceptionistsAsync(SearchParams searchParams)
    {
        var query = _context.Receptionists
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        return await PagedList<Receptionist>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);
    }

    public Task<Receptionist?> GetReceptionistByIdAsync(string id)
    {
        var result = _context.Receptionists.SingleOrDefault(x => x.Id == id);
        
        return Task.FromResult(result); 
    }

    public void DeleteReceptionist(Receptionist receptionist)
    {
        _context.Receptionists.RemoveRange(receptionist);
    }

    public Task<ReceptionistProfileByIdResponse> MappingToReceptionistProfileByIdResponse(Receptionist receptionist)
    {
        var result = _mapper.Map<Receptionist, ReceptionistProfileByIdResponse>(receptionist);

        return Task.FromResult(result);
    }

    public Task<ICollection<ReceptionistProfileResponse>> MappingToCollectionReceptionistProfileResponse(PagedList<Receptionist> receptionists)
    {
        List<ReceptionistProfileResponse> receptionistsList = new List<ReceptionistProfileResponse>();

        foreach (var receptionist in receptionists)
        {
            receptionistsList.Add(_mapper.Map<Receptionist, ReceptionistProfileResponse>(receptionist));
        }

        return Task.FromResult<ICollection<ReceptionistProfileResponse>>(receptionistsList);
    }
    
    public async Task CreatePatientProfileAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    public Task<Patient?> GetPatientByIdAsync(string id)
    {
        var patient = _context.Patients.SingleOrDefault(x => x.Id == id);
        
        return Task.FromResult(patient);
    }

    public Task<PatientProfileByDoctorResponse> MappingToPatientProfileByDoctorResponse(Patient patient)
    {
        var result = _mapper.Map<Patient, PatientProfileByDoctorResponse>(patient);

        return Task.FromResult(result);
    }

    public Task<PatientProfileByAdminResponse> MappingToPatientProfileByAdminResponse(Patient patient)
    {
        var result = _mapper.Map<Patient, PatientProfileByAdminResponse>(patient);

        return Task.FromResult(result);
    }

    public async Task<PagedList<Patient>> GetPatientsByAdminAsync(SearchParams searchParams)
    {
        var query = _context.Patients
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        return await PagedList<Patient>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);
    }

    public Task<ICollection<PatientsProfileSearchByAdminResponse>> MappingToPatientsProfileSearchByAdminResponse(PagedList<Patient> patients)
    {
        List<PatientsProfileSearchByAdminResponse> patientsList = new List<PatientsProfileSearchByAdminResponse>();

        foreach (var patient in patients)
        {
            patientsList.Add(_mapper.Map<Patient, PatientsProfileSearchByAdminResponse>(patient));
        }

        return Task.FromResult<ICollection<PatientsProfileSearchByAdminResponse>>(patientsList);
    }

    public void DeletePatient(Patient patient)
    {
        _context.Patients.RemoveRange(patient);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}