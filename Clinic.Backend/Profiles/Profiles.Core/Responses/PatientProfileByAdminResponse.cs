namespace Profiles.Core.Responses;

public record PatientProfileByAdminResponse(string FirstName, string LastName, string? MiddleName, string PhoneNumber, DateTime DateOfBirth);