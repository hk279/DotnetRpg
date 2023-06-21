namespace dotnet_rpg.Models;

public class Fight
{
    public int Id { get; init; }
    public List<Character> Characters { get; init; } = null!;
}

