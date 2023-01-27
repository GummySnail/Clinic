using AutoMapper;
using MassTransit;
using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Exceptions;
using Profiles.Core.Interfaces.Services;
using Profiles.Core.Pagination;
using Profiles.Core.Responses;
using Profiles.Infrastructure.Data;
using SharedModels;

namespace Profiles.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly ProfileDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    public ProfileService(ProfileDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, string phoneNumber)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth, phoneNumber);
        
        await _context.Patients.AddAsync(patient);

        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish<PatientProfileCreated>(new
        {
            patient.Id,
            patient.FirstName,
            patient.LastName,
            patient.MiddleName,
            patient.DateOfBirth,
            patient.PhoneNumber
        });
    }

    public async Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status)
    {
        var doctor = new Doctor(firstName, lastName, middleName, dateOfBirth, careerStartYear, status);
        
        await _context.Doctors.AddAsync(doctor);
        
        await _context.SaveChangesAsync();
    }

    public async Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName)
    {
        var receptionist = new Receptionist(firstName, lastName, middleName);
        
        await _context.Receptionists.AddAsync(receptionist);
        
        await _context.SaveChangesAsync();
    }

    public async Task CreatePatientProfileByAdminAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth)
    {
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth);
        
        await _context.Patients.AddAsync(patient);
        
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<DoctorProfileResponse>> GetDoctorsAtWorkAsync(SearchParams searchParams)
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

        var doctors = await PagedList<Doctor>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);

        List<DoctorProfileResponse> doctorsList = new List<DoctorProfileResponse>();

        foreach (var doctor in doctors)
        {
            doctorsList.Add(_mapper.Map<Doctor, DoctorProfileResponse>(doctor));
        }

        return doctorsList;
    }

    public async Task<ICollection<DoctorProfileSearchByAdminResponse>> GetDoctorsByAdminAsync(SearchParams searchParams)
    {
        var query = _context.Doctors
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        var doctors = await PagedList<Doctor>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);
        
        List<DoctorProfileSearchByAdminResponse> doctorsList = new List<DoctorProfileSearchByAdminResponse>();

        foreach (var doctor in doctors)
        {
            doctorsList.Add(_mapper.Map<Doctor, DoctorProfileSearchByAdminResponse>(doctor));
        }

        return doctorsList;
    }

    public async Task<ICollection<ReceptionistProfileResponse>> GetReceptionistsAsync(SearchParams searchParams)
    {
        var query = _context.Receptionists
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        var receptionists = await PagedList<Receptionist>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);

        List<ReceptionistProfileResponse> receptionistsList = new List<ReceptionistProfileResponse>();

        foreach (var receptionist in receptionists)
        {
            receptionistsList.Add(_mapper.Map<Receptionist, ReceptionistProfileResponse>(receptionist));
        }

        return receptionistsList;
    }

    public async Task<ICollection<PatientsProfileSearchByAdminResponse>> GetPatientsByAdminAsync(SearchParams searchParams)
    {
        var query = _context.Patients
            .Where(d => d.FirstName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.LastName.ToLower().Contains(searchParams.FullName.ToLower()) ||
                        d.MiddleName.ToLower().Contains(searchParams.FullName.ToLower()));

        var patients = await PagedList<Patient>
            .CreateAsync(query, searchParams.PageNumber, searchParams.PageSize);

        List<PatientsProfileSearchByAdminResponse> patientsList = new List<PatientsProfileSearchByAdminResponse>();

        foreach (var patient in patients)
        {
            patientsList.Add(_mapper.Map<Patient, PatientsProfileSearchByAdminResponse>(patient));
        }

        return patientsList;
    }

    public Task<PatientProfileByDoctorResponse> DoctorGetPatientProfileByIdAsync(string id)
    {
        var patient = _context.Patients.SingleOrDefault(x => x.Id == id);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }
        
        var result = _mapper.Map<Patient, PatientProfileByDoctorResponse>(patient);

        return Task.FromResult(result);
    }

    public Task<PatientProfileByAdminResponse> AdminGetPatientProfileByIdAsync(string id)
    {
        var patient = _context.Patients.SingleOrDefault(x => x.Id == id);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }
        
        var result = _mapper.Map<Patient, PatientProfileByAdminResponse>(patient);

        return Task.FromResult(result);
    }
    
    public Task<DoctorProfileResponse> GetDoctorProfileByIdAsync(string id)
    {
        var doctor = _context.Doctors.SingleOrDefault(x => x.Id == id);
        
        if (doctor is null)
        {
            throw new NotFoundException("Doctor is not exist");
        }

        var result = _mapper.Map<Doctor, DoctorProfileResponse>(doctor);

        return Task.FromResult(result);
    }

    public Task<ReceptionistProfileByIdResponse> GetReceptionistProfileByIdAsync(string id)
    {
        var receptionist = _context.Receptionists.SingleOrDefault(x => x.Id == id);

        if (receptionist is null)
        {
            throw new NotFoundException("Receptionist is not exist");
        }
        
        var result = _mapper.Map<Receptionist, ReceptionistProfileByIdResponse>(receptionist);

        return Task.FromResult(result);
    }

    public async Task DeletePatientProfileAsync(string id)
    {
        var patient = _context.Patients.SingleOrDefault(x => x.Id == id);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }
        
        _context.Patients.RemoveRange(patient);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReceptionistProfileAsync(string id)
    {
        var receptionist = _context.Receptionists.SingleOrDefault(x => x.Id == id);

        if (receptionist is null)
        {
            throw new NotFoundException("Receptionist is not exist");
        }
        
        _context.Receptionists.RemoveRange(receptionist);

        await _context.SaveChangesAsync();
    }

    public async Task ChangeDoctorStatusAsync(string id, Status status)
    {
        var doctor = _context.Doctors.SingleOrDefault(x => x.Id == id);

        if (doctor is null)
        {
            throw new NotFoundException("Doctor is not exist");
        }
        
        doctor.Status = status;
        
        await _context.SaveChangesAsync();
    }
}