namespace Profiles.Core.Interfaces.Data.Repositories;

public interface IRepositoryManager
{
    IDoctorRepository DoctorRepository { get; }
    IPatientRepository PatientRepository { get; }
    IReceptionistRepository ReceptionistRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}