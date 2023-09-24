namespace dotnet_rpg.Models.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string? message)
        : base(message) { }
}
