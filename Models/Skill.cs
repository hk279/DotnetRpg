namespace dotnet_rpg.Models;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Damage { get; set; }
    public int Healing { get; set; }
    public SkillDamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; } = SkillTargetType.Enemy;
    public CharacterClass CharacterClass { get; set; }
    public List<Character> Characters { get; set; } = new List<Character>();

    // TODO: Skill cooldowns
}
