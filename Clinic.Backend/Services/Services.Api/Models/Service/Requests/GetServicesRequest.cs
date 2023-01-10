using Services.Core.Enums;

namespace Services.Api.Models.Service.Requests;

public record GetServicesRequest(Category Category = Category.Consultation);