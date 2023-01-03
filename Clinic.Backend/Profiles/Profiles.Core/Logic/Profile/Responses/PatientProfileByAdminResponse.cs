namespace Profiles.Core.Logic.Profile.Responses;

public record PatientProfileByAdminResponse(string firstName, string lastName, string? middleName, string phoneNumber, DateTime dateOfBirth);