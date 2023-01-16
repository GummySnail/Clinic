using System.Net;

namespace Documents.Api.Models.Responses;

public class ValidationErrorResponse
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;
    public List<ValidationErrorModel> Errors { get; set; } = new List<ValidationErrorModel>();
}