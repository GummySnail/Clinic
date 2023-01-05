using Services.Core.Enums;

namespace Services.Core.Entities;

public class ServiceCategory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Category CategoryName { get; set; }
    public string TimeSlotSize { get; set; }
    public ICollection<Service> Services { get; set; }
}