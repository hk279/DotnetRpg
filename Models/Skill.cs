namespace dotnet_rpg.Models;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Damage { get; set; }
    public int Healing { get; set; }
    public SkillType SkillType { get; set; }
    public SkillTargetType SkillTargetType { get; set; } = SkillTargetType.Enemy;
    public CharacterClass CharacterClass { get; set; }
    public List<Character> Characters { get; set; } = new List<Character>();

    // TODO: Skill cooldowns
}
