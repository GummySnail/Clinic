using AutoMapper;
using Offices.Core.Entities;
using Offices.Core.Logic.Responses;

namespace Offices.Infrastructure.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Office, OfficesResponse>();
        CreateMap<Office, OfficeResponse>();
    }
}