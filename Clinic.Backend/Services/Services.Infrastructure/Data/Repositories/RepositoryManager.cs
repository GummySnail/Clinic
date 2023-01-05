using Services.Core.Interfaces.Data.Repositories;

namespace Services.Infrastructure.Data.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IServiceRepository> _lazyServiceRepository;
    private readonly Lazy<IServiceCategoryRepository> _lazyServiceCategoryRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(ServicesDbContext context)
    {
        _lazyServiceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(context));
        _lazyServiceCategoryRepository = new Lazy<IServiceCategoryRepository>(() => new ServiceCategoryRepository(context));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
    }

    public IServiceRepository ServiceRepository => _lazyServiceRepository.Value;
    public IServiceCategoryRepository ServiceCategoryRepository => _lazyServiceCategoryRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}