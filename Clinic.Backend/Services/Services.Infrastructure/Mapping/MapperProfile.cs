using AutoMapper;
using Services.Core.Entities;
using Services.Core.Responses;

namespace Services.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Service, GetServicesByCategoryResponse>()
            .ConstructUsing(x => new GetServicesByCategoryResponse(x.ServiceName, x.Price, x.IsActive));

        CreateMap<Specialization, GetSpecializationsListResponse>()
            .ConstructUsing(x => new GetSpecializationsListResponse(x.SpecializationName, x.IsActive));

        CreateMap<Service, GetServiceResponse>()
            .ConstructUsing(x => new GetServiceResponse(x.ServiceName, x.Price, x.Category, x.IsActive));
    }
}