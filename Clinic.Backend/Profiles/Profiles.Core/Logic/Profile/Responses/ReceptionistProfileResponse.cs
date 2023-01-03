namespace Profiles.Core.Logic.Profile.Responses;

public class ReceptionistProfileResponse
{
    public ReceptionistProfileResponse(string firstName, string lastName, string? middleName)
    {
        FullName = $"{firstName} {lastName} {middleName}";
    }

    public string FullName { get; set; }
}