using AutoMapper;
using Profiles.Core.Entities;
using Profiles.Core.Responses;

namespace Profiles.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Doctor, DoctorProfileResponse>()
            .ConstructUsing(x => new DoctorProfileResponse(x.FirstName, x.LastName, x.MiddleName, x.CareerStartYear));

        CreateMap<Doctor, DoctorProfileSearchByAdminResponse>()
            .ConstructUsing(x =>
                new DoctorProfileSearchByAdminResponse(x.FirstName, x.LastName, x.MiddleName, x.Status, x.DateOfBirth));

        CreateMap<Receptionist, ReceptionistProfileResponse>()
            .ConstructUsing(x => new ReceptionistProfileResponse(x.FirstName, x.LastName, x.MiddleName));

        CreateMap<Patient, PatientsProfileSearchByAdminResponse>()
            .ConstructUsing(x =>
                new PatientsProfileSearchByAdminResponse(x.FirstName, x.LastName, x.MiddleName, x.PhoneNumber));

        CreateMap<Patient, PatientProfileByDoctorResponse>();

        CreateMap<Patient, PatientProfileByAdminResponse>();

        CreateMap<Receptionist, ReceptionistProfileByIdResponse>();

    }
}