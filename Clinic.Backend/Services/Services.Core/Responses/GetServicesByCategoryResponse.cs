namespace Services.Core.Responses;

public record GetServicesByCategoryResponse(string ServiceName, float Price, bool IsActive);