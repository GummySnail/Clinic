namespace Profiles.Core.Logic.Profile.Responses;

public class DoctorProfileResponse
{
    public DoctorProfileResponse(string firstName, string lastName, string? middleName, int careerStartYear)
    {
        FullName = $"{firstName} {lastName} {middleName}";
        Experience = DateTime.UtcNow.Year - careerStartYear + 1;
    }
    public string FullName { get; set; }
    public int Experience { get; set; }
}