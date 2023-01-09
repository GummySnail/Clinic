namespace Profiles.Core.Responses;

public record PatientProfileByDoctorResponse(string FirstName, string LastName, string? MiddleName, string PhoneNumber, DateTime DateOfBirth);