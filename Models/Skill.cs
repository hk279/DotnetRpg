namespace dotnet_rpg.Models;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Damage { get; set; }
    public int Healing { get; set; }
    public int EnergyCost { get; set; }
    public int Cooldown { get; set; } // TODO: Implement cooldown trigger and remaining turns
    public DamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; } = SkillTargetType.Enemy;
    public CharacterClass CharacterClass { get; set; }
    public List<Character> Characters { get; set; } = new List<Character>();
}
