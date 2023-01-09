namespace Services.Api.Models.Service.Requests;

public record SpecializationRequest(string SpecializationName, bool IsActive, string ServiceId);