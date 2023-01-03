using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Offices.Core.Entities;

public class Office
{
    public Office(string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber, bool isActive)
    {
        Address = $"{city} {street} {houseNumber} {officeNumber}";
        RegistryPhoneNumber = registryPhoneNumber;
        IsActive = isActive;
    }
    
    [BsonId]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Address { get; set; }
    public string RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; }
}