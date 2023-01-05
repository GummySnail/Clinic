namespace Services.Core.Interfaces.Data.Repositories;

public interface IRepositoryManager
{
    IServiceRepository ServiceRepository { get; }
    IServiceCategoryRepository ServiceCategoryRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}