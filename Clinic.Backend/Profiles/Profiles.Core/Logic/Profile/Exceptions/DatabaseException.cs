namespace Profiles.Core.Logic.Profile.Exceptions;

public class DatabaseException : Exception
{
    public DatabaseException(string message) : base(message) { }
}