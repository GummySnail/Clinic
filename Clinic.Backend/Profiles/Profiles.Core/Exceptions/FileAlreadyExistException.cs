﻿namespace Profiles.Core.Exceptions;

public class FileAlreadyExistException : Exception
{
    public FileAlreadyExistException(string message) : base(message) { }
}