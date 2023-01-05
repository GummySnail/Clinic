namespace Services.Core.Entities;

public class Service
{
    public Service(string serviceName, float price, string id, bool isActive)
    {
        ServiceName = serviceName;
        Price = price;
        CategoryId = id;
        IsActive = isActive;
    }
    
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ServiceName { get; set; }
    public float Price { get; set; }
    public bool IsActive { get; set; }
    public string CategoryId { get; set; }
    public ServiceCategory Category { get; set; }
    public string SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
}