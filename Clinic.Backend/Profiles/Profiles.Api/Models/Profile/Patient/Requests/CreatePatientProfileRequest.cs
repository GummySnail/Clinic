namespace Profiles.Api.Models.Profile.Patient.Requests;

public record CreatePatientProfileRequest(string FirstName, string LastName, string? MiddleName, DateTime DateOfBirth, string PhoneNumber);