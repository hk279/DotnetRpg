using System.Text.Json.Serialization;

namespace dotnet_rpg.Models;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public int Healing { get; set; }
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

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SkillTargetType
{
    Unknown = 0,
    Self = 1,
    Friendly = 2,
    Enemy = 3
}
