using Profiles.Core.Enums;

namespace Profiles.Core.Entities;

public class Doctor
{
    public Doctor(string firstName, string lastName, string? middleName,
        DateTime dateOfBirth, int careerStartYear, Status status, string? url)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth.Date;
        CareerStartYear = careerStartYear;
        Status = status;
        Url = url;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int CareerStartYear { get; set; }
    public Status Status { get; set; }
    public string? Url { get; set; }
}