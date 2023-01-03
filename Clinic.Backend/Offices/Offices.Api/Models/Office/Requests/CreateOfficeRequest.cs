namespace Offices.Api.Models.Office.Requests;

public record CreateOfficeRequest(string City, string Street, string HouseNumber, string OfficeNumber, string RegistryPhoneNumber, bool IsActive);