namespace Offices.Api.Models.Office.Requests;

public record OfficeRequest(string City, string Street, string HouseNumber, string OfficeNumber, string RegistryPhoneNumber, bool IsActive, IFormFile? officePhoto);