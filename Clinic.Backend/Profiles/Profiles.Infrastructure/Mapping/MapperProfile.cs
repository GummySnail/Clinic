using AutoMapper;
using Profiles.Core.Entities;
using Profiles.Core.Logic.Profile.Responses;

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
    }
}