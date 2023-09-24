namespace dotnet_rpg.Models.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string? message)
        : base(message) { }
}
