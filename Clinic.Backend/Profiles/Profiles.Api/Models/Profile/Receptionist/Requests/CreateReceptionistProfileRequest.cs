namespace Profiles.Api.Models.Profile.Receptionist.Requests;

public record CreateReceptionistProfileRequest(string FirstName, string LastName, string? MiddleName, IFormFile? ProfilePhoto);