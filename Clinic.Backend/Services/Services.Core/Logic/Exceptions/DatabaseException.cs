namespace Services.Core.Logic.Exceptions;

public class DatabaseException : Exception
{
    public DatabaseException(string message) : base(message) { }
}