namespace Services.Core.Entities;

public class Specialization
{
    public Specialization(string specializationName, bool isActive)
    {
        SpecializationName = specializationName;
        IsActive = isActive;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SpecializationName { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Service> Services { get; set; }
}