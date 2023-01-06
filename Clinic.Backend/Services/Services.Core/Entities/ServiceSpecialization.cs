namespace Services.Core.Entities;

public class ServiceSpecialization
{
    public string ServiceId { get; set; }
    public Service Service { get; set; }
    public string SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
}