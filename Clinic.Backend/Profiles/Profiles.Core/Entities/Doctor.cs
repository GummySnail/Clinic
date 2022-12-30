using Profiles.Core.Enums;

namespace Profiles.Core.Entities;

public class Doctor
{
    public Doctor(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth.Date;
        CareerStartYear = careerStartYear;
        Status = status;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int CareerStartYear { get; set; }
    public Status Status { get; set; }
}