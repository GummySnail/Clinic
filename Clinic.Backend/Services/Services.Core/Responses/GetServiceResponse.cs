using Services.Core.Entities;
using Services.Core.Enums;

namespace Services.Core.Responses;

public class GetServiceResponse
{
    public GetServiceResponse(string serviceName, float price, ServiceCategory category, bool isActive)
    {
        ServiceName = serviceName;
        Price = price;
        CategoryName = category.CategoryName;
        IsActive = isActive;
    }

    public string ServiceName { get; set; }
    public float Price { get; set; }
    public Category CategoryName { get; set; }
    public bool IsActive { get; set; }
}