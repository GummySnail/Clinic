using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Offices.Core.Entities;
using Offices.Core.Interfaces.Data.Repositories;
using Offices.Core.Logic.Responses;
using Offices.Infrastructure.Models;
using IMapper = AutoMapper.IMapper;

namespace Offices.Infrastructure.Data.Repositories;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<Office> _officesCollection;
    private readonly IMapper _mapper;

    public OfficeRepository(IOptions<OfficeDatabaseSettings> officeDatabaseSettings, IMapper mapper)
    {
        var mongoClient = new MongoClient(officeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(officeDatabaseSettings.Value.DatabaseName);
        _officesCollection = mongoDatabase.GetCollection<Office>(officeDatabaseSettings.Value.OfficesCollectionName);
        _mapper = mapper;
    }

    public async Task<List<Office>> GetOfficesCollectionAsync()
    {
        return await _officesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Office> GetOfficeByIdAsync(string id)
    {
        return await _officesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task ChangeOfficeStatusAsync(string id, bool isActive)
    {
        var filter = Builders<Office>.Filter.Eq("_id", id);

        var update = Builders<Office>.Update.Set("IsActive", isActive);
        
        await _officesCollection.UpdateOneAsync(filter, update);
    }

    public async Task EditOfficeAsync(string id, Office newOffice)
    {
        newOffice.Id = id;
        await _officesCollection.ReplaceOneAsync(x => x.Id == id, newOffice);
    }

    public Task<OfficeResponse> MappingToOfficeResponse(Office office)
    {
        var result = _mapper.Map<Office, OfficeResponse>(office);

        return Task.FromResult(result);
    }

    public Task<ICollection<OfficesResponse>> MappingToCollectionOfficesResponse(List<Office> offices)
    {
        List<OfficesResponse> officesList = new List<OfficesResponse>();

        foreach (var office in offices)
        {
            officesList.Add(_mapper.Map<Office, OfficesResponse>(office));
        }

        return Task.FromResult<ICollection<OfficesResponse>>(officesList);
    }

    public async Task CreateAsync(Office office)
    {
        await _officesCollection.InsertOneAsync(office);
    }
}