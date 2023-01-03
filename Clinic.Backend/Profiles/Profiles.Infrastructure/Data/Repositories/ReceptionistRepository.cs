using AutoMapper;
using Profiles.Core.Entities;
using Profiles.Core.Interfaces.Data.Repositories;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Infrastructure.Data.Repositories;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfileDbContext _context;
    private readonly IMapper _mapper;

    public ReceptionistRepository(ProfileDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<ICollection<ReceptionistProfileResponse>> MappingToReceptionistProfileResponse(PagedList<Receptionist> receptionists)
    {
        List<ReceptionistProfileResponse> receptionistsList = new List<ReceptionistProfileResponse>();

        foreach (var receptionist in receptionists)
        {
            receptionistsList.Add(_mapper.Map<Receptionist, ReceptionistProfileResponse>(receptionist));
        }

        return receptionistsList;
    }
}