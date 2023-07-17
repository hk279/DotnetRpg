namespace dotnet_rpg.Models;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Damage { get; set; } = 0;
    public int Healing { get; set; } = 0;
    public int EnergyCost { get; set; }
    public int Cooldown { get; set; }
    public int RemainingCooldown { get; set; }
    public DamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; } = SkillTargetType.Enemy;
    public CharacterClass CharacterClass { get; set; }

    public void ApplyCooldown()
    {
        RemainingCooldown = Cooldown;
    }
}
