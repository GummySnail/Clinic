using Profiles.Core.Enums;

namespace Profiles.Api.Models.Profile.Doctor.Requests;

public record CreateDoctorProfileRequest(string FirstName, string LastName, string? MiddleName,
    DateTime DateOfBirth, int CareerStartYear, Status Status);