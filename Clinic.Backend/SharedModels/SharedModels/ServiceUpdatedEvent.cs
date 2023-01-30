namespace SharedModels;

public class ServiceUpdatedEvent
{
    public string Id { get; set; }
    public string ServiceName { get; set; }
    public float Price { get; set; }
    public bool IsActive { get; set; }
    public string CategoryId { get; set; }
}