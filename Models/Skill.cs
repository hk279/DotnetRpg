namespace dotnet_rpg.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Damage { get; set; }
    public DamageType DamageType { get; set; }
    public List<Character> Characters { get; set; } = null!;
}
