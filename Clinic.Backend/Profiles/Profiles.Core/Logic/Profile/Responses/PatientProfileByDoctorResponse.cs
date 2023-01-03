namespace Profiles.Core.Logic.Profile.Responses;

public record PatientProfileByDoctorResponse(string firstName, string lastName, string? middleName, string phoneNumber, DateTime dateOfBirth);