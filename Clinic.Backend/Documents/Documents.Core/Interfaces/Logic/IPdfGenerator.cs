namespace Documents.Core.Interfaces.Logic;

public interface IPdfGenerator
{
    public Task<byte[]> CreatePdfAsync(string complaints, string conclusion, string recommendations);
}