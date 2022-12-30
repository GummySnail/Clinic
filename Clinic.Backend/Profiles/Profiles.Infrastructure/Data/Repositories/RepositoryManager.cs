using AutoMapper;
using Profiles.Core.Interfaces.Data.Repositories;

namespace Profiles.Infrastructure.Data.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IReceptionistRepository> _lazyReceptionistRepository;
    private readonly Lazy<IPatientRepository> _lazyPatientRepository;
    private readonly Lazy<IDoctorRepository> _lazyDoctorRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(ProfileDbContext context, IMapper mapper)
    {
        _lazyReceptionistRepository = new Lazy<IReceptionistRepository>(() => new ReceptionistRepository(context));
        _lazyPatientRepository = new Lazy<IPatientRepository>(() => new PatientRepository(context));
        _lazyDoctorRepository = new Lazy<IDoctorRepository>(() => new DoctorRepository(context, mapper));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
    }

    public IDoctorRepository DoctorRepository => _lazyDoctorRepository.Value;
    public IPatientRepository PatientRepository => _lazyPatientRepository.Value;
    public IReceptionistRepository ReceptionistRepository => _lazyReceptionistRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}