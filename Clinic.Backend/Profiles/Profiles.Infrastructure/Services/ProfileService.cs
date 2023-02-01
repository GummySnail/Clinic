using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Profiles.Core.Entities;
using Profiles.Core.Enums;
using Profiles.Core.Exceptions;
using Profiles.Core.Interfaces.Services;
using Profiles.Core.Pagination;
using Profiles.Core.Responses;
using Profiles.Infrastructure.Data;

namespace Profiles.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly ProfileDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;
    public ProfileService(ProfileDbContext context, IMapper mapper, IAzureService azureService)
    {
        _context = context;
        _mapper = mapper;
        _azureService = azureService;
    }

    public async Task CreatePatientProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, string phoneNumber, IFormFile? profilePhoto)
    {
        string url = null;
        
        if (profilePhoto is not null)
        {
            url = await _azureService.UploadProfilePhotoAsync(profilePhoto);
        }
        
        var patient = new Patient(firstName, lastName, middleName, dateOfBirth, phoneNumber, url);
        
        await _context.Patients.AddAsync(patient);

        await _context.SaveChangesAsync();
    }

    public async Task CreateDoctorProfileAsync(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status, IFormFile? profilePhoto)
    {
        string url = null;
        
        if (profilePhoto is not null)
        {
            url = await _azureService.UploadProfilePhotoAsync(profilePhoto);
        }

        var doctor = new Doctor(firstName, lastName, middleName, dateOfBirth, careerStartYear, status, url);
        
        await _context.Doctors.AddAsync(doctor);
        
        await _context.SaveChangesAsync();
    }

    public async Task CreateReceptionistProfileAsync(string firstName, string lastName, string? middleName, IFormFile? profilePhoto)
    {
        string url = null;
        
        if (profilePhoto is not null)
        {
            url = await _azureService.UploadProfilePhotoAsync(profilePhoto);
        }
        
        var receptionist = new Receptionist(firstName, lastName, middleName, url);
        
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

    public async Task<PatientProfileByDoctorResponse> DoctorGetPatientProfileByIdAsync(string patientId)
    {
        var patient = await _context.Patients.SingleOrDefaultAsync(x => x.Id == patientId);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }
        
        var result = _mapper.Map<Patient, PatientProfileByDoctorResponse>(patient);

        return result;
    }

    public async Task<PatientProfileByAdminResponse> AdminGetPatientProfileByIdAsync(string patientId)
    {
        var patient = await _context.Patients.SingleOrDefaultAsync(x => x.Id == patientId);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }
        
        var result = _mapper.Map<Patient, PatientProfileByAdminResponse>(patient);

        return result;
    }
    
    public async Task<DoctorProfileResponse> GetDoctorProfileByIdAsync(string doctorId)
    {
        var doctor = await _context.Doctors.SingleOrDefaultAsync(x => x.Id == doctorId);
        
        if (doctor is null)
        {
            throw new NotFoundException("Doctor is not exist");
        }

        var result = _mapper.Map<Doctor, DoctorProfileResponse>(doctor);

        return result;
    }

    public async Task<ReceptionistProfileByIdResponse> GetReceptionistProfileByIdAsync(string receptionistId)
    {
        var receptionist = await _context.Receptionists.SingleOrDefaultAsync(x => x.Id == receptionistId);

        if (receptionist is null)
        {
            throw new NotFoundException("Receptionist is not exist");
        }
        
        var result = _mapper.Map<Receptionist, ReceptionistProfileByIdResponse>(receptionist);

        return result;
    }

    public async Task DeletePatientProfileAsync(string patientId)
    {
        var patient = await _context.Patients.SingleOrDefaultAsync(x => x.Id == patientId);

        if (patient is null)
        {
            throw new NotFoundException("Patient is not exist");
        }
        
        _context.Patients.RemoveRange(patient);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReceptionistProfileAsync(string receptionistId)
    {
        var receptionist = await _context.Receptionists.SingleOrDefaultAsync(x => x.Id == receptionistId);

        if (receptionist is null)
        {
            throw new NotFoundException("Receptionist is not exist");
        }
        
        _context.Receptionists.RemoveRange(receptionist);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateDoctorStatusAsync(string doctorId, Status status)
    {
        var doctor = await _context.Doctors.SingleOrDefaultAsync(x => x.Id == doctorId);

        if (doctor is null)
        {
            throw new NotFoundException("Doctor is not exist");
        }
        
        doctor.Status = status;
        
        await _context.SaveChangesAsync();
    }
}