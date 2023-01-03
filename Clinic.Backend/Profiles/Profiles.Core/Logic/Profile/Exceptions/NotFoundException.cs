namespace Profiles.Core.Logic.Profile.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}