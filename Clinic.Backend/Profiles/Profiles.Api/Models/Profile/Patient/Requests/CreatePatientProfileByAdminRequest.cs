namespace Profiles.Api.Models.Profile.Patient.Requests;

public record CreatePatientProfileByAdminRequest(string FirstName, string LastName, string? MiddleName, DateTime DateOfBirth);