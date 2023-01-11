namespace Services.Core.Responses;

public record GetServicesResponse(string ServiceName, float Price, bool IsActive);