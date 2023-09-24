namespace dotnet_rpg.Models.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string? message)
        : base(message) { }
}
