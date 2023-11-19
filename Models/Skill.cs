using System.Text.Json.Serialization;

namespace dotnet_rpg.Models;

public class Skill
{
    public Skill() { }

    public Skill(
        CharacterClass characterClass,
        string name,
        string description,
        DamageType damageType,
        SkillTargetType targetType,
        int rank,
        int weaponDamagePercentage = 0,
        int minBaseDamageFactor = 0,
        int maxBaseDamageFactor = 0,
        int baseDamageAttributeScalingFactor = 0,
        int healing = 0,
        int energyCost = 0,
        int cooldown = 0,
        StatusEffect? statusEffect = null
    )
    {
        CharacterClass = characterClass;
        Name = name;
        Description = description;
        DamageType = damageType;
        TargetType = targetType;
        WeaponDamagePercentage = weaponDamagePercentage;
        MinBaseDamageFactor = minBaseDamageFactor;
        Rank = rank;
        MaxBaseDamageFactor = maxBaseDamageFactor;
        BaseDamageAttributeScalingFactor = baseDamageAttributeScalingFactor;
        Healing = healing;
        EnergyCost = energyCost;
        Cooldown = cooldown;
        StatusEffect = statusEffect;
    }

    public int Id { get; set; }
    public CharacterClass CharacterClass { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; }
    public int Rank { get; set; } = 1;

    /// <summary>
    /// Determines how much of the equipped weapon damage is included in the total damage of the skill.
    /// </summary>
    public int WeaponDamagePercentage { get; set; }

    // Base damage component of the skill is scaled by player level, attributes, attribute scaling factor and min / max damage factor.

    public int MinBaseDamageFactor { get; set; } // 1-100
    public int MaxBaseDamageFactor { get; set; } // 1-100
    public int BaseDamageAttributeScalingFactor { get; set; } // 1-100

    public int Healing { get; set; } // TODO: Implement healing

    public StatusEffect? StatusEffect { get; set; }

    public int EnergyCost { get; set; }
    public int Cooldown { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SkillTargetType
{
    Unknown,
    Self,
    Friendly,
    Enemy,
    AllEnemies
}
