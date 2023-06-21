namespace dotnet_rpg.Models;

public class Fight
{
    public int Id { get; init; }
    public Character PlayerCharacter { get; init; } = null!;
    public List<Character> Enemies { get; init; } = null!;
    public int MyProperty { get; set; }
    public FightStatus FightStatus { get; set; }
    public bool IsPlayersTurn { get; set; }
}

