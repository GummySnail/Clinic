﻿using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Offices.Core.Entities;
using Offices.Core.Exceptions;
using Offices.Core.Interfaces.Services;
using Offices.Core.Responses;
using Offices.Infrastructure.Models;

namespace Offices.Infrastructure.Services;

public class OfficeService : IOfficeService
{
    private readonly IMongoCollection<Office> _officesCollection;
    private readonly IMapper _mapper;

    public OfficeService(IOptions<OfficeDatabaseSettings> officeDatabaseSettings, IMapper mapper)
    {
        var mongoClient = new MongoClient(officeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(officeDatabaseSettings.Value.DatabaseName);
        _officesCollection = mongoDatabase.GetCollection<Office>(officeDatabaseSettings.Value.OfficesCollectionName);
        _mapper = mapper;
    }

    public async Task<ICollection<OfficeCollectionResponse>> GetOfficesCollectionAsync()
    {
        var offices = await _officesCollection.Find(_ => true).ToListAsync();
        
        List<OfficeCollectionResponse> officeList = new List<OfficeCollectionResponse>();

        foreach (var office in offices)
        {
            officeList.Add(_mapper.Map<Office, OfficeCollectionResponse>(office));
        }
        
        return officeList;
    }

    public async Task CreateAsync(string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber, bool isActive)
    {
        var office = new Office(city, street, houseNumber, officeNumber, registryPhoneNumber, isActive);
        
        await _officesCollection.InsertOneAsync(office);
    }

    public async Task<OfficeResponse> GetOfficeByIdAsync(string id)
    {
        var office = await _officesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (office is null)
        {
            throw new NotFoundException("Office is not exist");
        }
        
        var result = _mapper.Map<Office, OfficeResponse>(office);

        return result;
    }

    public async Task ChangeOfficeStatusAsync(string id, bool isActive)
    {
        var filter = Builders<Office>.Filter.Eq("_id", id);

        var update = Builders<Office>.Update.Set("IsActive", isActive);
        
        var result = await _officesCollection.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0) 
        {
            throw new NotFoundException("Office is not exist");
        }
    }

    public async Task EditOfficeAsync(string id, string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber, bool isActive)
    {
        var newOffice = new Office(city, street, houseNumber, officeNumber, registryPhoneNumber, isActive)
        {
            Id = id
        };

        var result = await _officesCollection.ReplaceOneAsync(x => x.Id == id, newOffice);

        if (result.MatchedCount == 0)
        {
            throw new NotFoundException("Office is not exist");
        }
    }
}