namespace Documents.Core.Entities;

public class Photo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Url { get; set; }
}