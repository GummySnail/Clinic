using Profiles.Core.Enums;

namespace Profiles.Core.Responses;

public class DoctorProfileSearchByAdminResponse
{
    public DoctorProfileSearchByAdminResponse(string firstName, string lastName, string? middleName, Status status, DateTime dateOfBirth)
    {
        FullName = $"{firstName} {lastName} {middleName}";
        Status = status;
        DateOfBirth = dateOfBirth.Date;
    }

    public string FullName { get; set; }
    public Status Status { get; set; }
    public DateTime DateOfBirth { get; set; }
}