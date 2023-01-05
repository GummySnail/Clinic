namespace Services.Core.Entities;

public class Specialization
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SpecializationName { get; set; }
    public bool IsActive { get; set; }
}