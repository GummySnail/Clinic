using Services.Core.Entities;
using Services.Core.Enums;

namespace Services.Core.Responses;

public class GetSpecializationResponse
{
    public GetSpecializationResponse(string specializationName, IEnumerable<float> price, IEnumerable<bool> isActive, IEnumerable<Category> serviceCategory)
    {
        SpecializationName = specializationName;
        Price = price.First();
        IsActive = isActive.First();
        ServiceCategoryName = serviceCategory.First();
    }

    public string SpecializationName { get; set; }
    public float Price { get; set; }
    public bool IsActive { get; set; }
    public Category ServiceCategoryName { get; set; }
}