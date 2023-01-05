namespace Services.Core.Logic.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) {}
}