namespace Profiles.Core.Entities;

public class Patient
{
    public Patient(string firstName, string lastName, string? middleName, DateTime dateOfBirth, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth.Date;
        PhoneNumber = phoneNumber;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public bool IsLinkedToAccount { get; set; } = true;
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    
}