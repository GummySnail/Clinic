﻿using AutoMapper;
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
}