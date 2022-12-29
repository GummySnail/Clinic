namespace Profiles.Core.Entities;

public class Patient
{
    public Patient(string firstName, string lastName, string? middleName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth.Date;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public bool IsLinkedToAccount { get; set; } = false;
    public DateTime DateOfBirth { get; set; }
    
}