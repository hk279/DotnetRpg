namespace DotnetRpg.Models.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string? message)
        : base(message) { }
}
