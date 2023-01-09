namespace Services.Core.Entities;

public class ServiceSpecialization
{
    public string SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
    public string ServiceId { get; set; }
    public Service Service { get; set; }
}