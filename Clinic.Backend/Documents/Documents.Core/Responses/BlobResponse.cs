using Documents.Core.Dto;

namespace Documents.Core.Responses;

public class BlobResponse
{
    public string? Status { get; set; }
    public bool Error { get; set; }
    public BlobDto Blob { get; set; }

    public BlobResponse()
    {
        Blob = new BlobDto();
    }
}