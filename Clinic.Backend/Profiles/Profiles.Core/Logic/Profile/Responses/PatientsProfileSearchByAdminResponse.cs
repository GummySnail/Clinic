namespace Profiles.Core.Logic.Profile.Responses;

public class PatientsProfileSearchByAdminResponse
{
    public PatientsProfileSearchByAdminResponse(string firstName, string lastName, string? middleName, string phoneNumber)
    {
        FullName = $"{firstName} {lastName} {middleName}";
        PhoneNumber = phoneNumber;
    }

    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}