namespace Documents.Core.Entities;

public class Document
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Url { get; set; }
}