using Profiles.Core.Entities;
using Profiles.Core.Logic;
using Profiles.Core.Logic.Profile.Responses;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IReceptionistRepository
{
    public Task CreateReceptionistProfileAsync(Receptionist receptionist);
    public Task<PagedList<Receptionist>> GetReceptionistsAsync(SearchParams searchParams);
    public Task<Receptionist?> GetReceptionistByIdAsync(string id);
    public void DeleteReceptionist(Receptionist receptionist);
    public Task<ReceptionistProfileByIdResponse> MappingToReceptionistProfileByIdResponse(Receptionist receptionist);
    public Task<ICollection<ReceptionistProfileResponse>> MappingToCollectionReceptionistProfileResponse(
        PagedList<Receptionist> receptionists);
}