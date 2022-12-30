using Profiles.Core.Entities;

namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IReceptionistRepository
{
    public Task CreateReceptionistProfileAsync(Receptionist receptionist);
}