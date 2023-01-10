using AutoMapper;
using Offices.Core.Entities;
using Offices.Core.Responses;

namespace Offices.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Office, OfficeCollectionResponse>();
        CreateMap<Office, OfficeResponse>();
    }
}