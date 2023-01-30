namespace Profiles.Core.Entities;

public class Receptionist
{
    public Receptionist(string firstName, string lastName, string? middleName, string? url)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Url = url;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Url { get; set; }
}