using AutoMapper;
using Services.Core.Entities;
using Services.Core.Responses;

namespace Services.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Service, GetServicesResponse>()
            .ConstructUsing(x => new GetServicesResponse(x.ServiceName, x.Price, x.IsActive));

        CreateMap<Specialization, GetSpecializationsResponse>()
            .ConstructUsing(x => new GetSpecializationsResponse(x.SpecializationName, x.IsActive));

        CreateMap<Service, GetServiceResponse>()
            .ConstructUsing(x => new GetServiceResponse(x.ServiceName, x.Price, x.Category, x.IsActive));
    }
}