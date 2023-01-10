using Services.Core.Enums;

namespace Services.Api.Models.Service.Requests;

public record ServiceRequest(string ServiceName, float Price, Category ServiceCategory, bool IsActive);