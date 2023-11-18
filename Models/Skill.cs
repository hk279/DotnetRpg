using System.Text.Json.Serialization;

namespace dotnet_rpg.Models;

public class Skill
{
    public Skill() { }

    public Skill(
        CharacterClass characterClass,
        string name,
        int rank,
        DamageType damageType,
        SkillTargetType targetType,
        int weaponDamagePercentage = 0,
        int minBaseDamageFactor = 0,
        int maxBaseDamageFactor = 0,
        int baseDamageAttributeScalingFactor = 0,
        int healing = 0,
        int energyCost = 0,
        int cooldown = 0,
        string description = "",
        StatusEffect? statusEffect = null
    )
    {
        CharacterClass = characterClass;
        Name = name;
        Rank = rank;
        WeaponDamagePercentage = weaponDamagePercentage;
        MinBaseDamageFactor = minBaseDamageFactor;
        MaxBaseDamageFactor = maxBaseDamageFactor;
        BaseDamageAttributeScalingFactor = baseDamageAttributeScalingFactor;
        Healing = healing;
        EnergyCost = energyCost;
        Cooldown = cooldown;
        DamageType = damageType;
        TargetType = targetType;
        Description = description;
        StatusEffect = statusEffect;
    }

    public int Id { get; set; }
    public CharacterClass CharacterClass { get; set; }
    public string Name { get; set; } = null!;
    public int Rank { get; set; } = 1;

    /// <summary>
    /// Determines how much of the equipped weapon damage is included in the total damage of the skill.
    /// </summary>
    public int WeaponDamagePercentage { get; set; }

    // Base damage component of the skill is scaled by player level, attributes, attribute scaling factor and min / max damage factor.

    public int MinBaseDamageFactor { get; set; } // 1-100
    public int MaxBaseDamageFactor { get; set; } // 1-100
    public int BaseDamageAttributeScalingFactor { get; set; } // 1-100

    public int Healing { get; set; }
    public StatusEffect? StatusEffect { get; set; }
    public int EnergyCost { get; set; }
    public int Cooldown { get; set; }
    public int RemainingCooldown { get; set; }
    public DamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; }
    public string Description { get; set; } = null!;

    public void ApplyCooldown()
    {
        RemainingCooldown = Cooldown;
    }

    public DamageRange GetSkillBaseDamageRange(Character character)
    {
        var scalingAttributeAmount = DamageType switch
        {
            DamageType.Physical => character.GetStrength(),
            DamageType.Magic => character.GetIntelligence(),
            _ => throw new ArgumentOutOfRangeException(nameof(DamageType), "Unknown damage type")
        };

        // TODO: Refine formulas
        var skillMinimumBaseDamage =
            character.Level
            * (MinBaseDamageFactor / 10)
            * (scalingAttributeAmount * (BaseDamageAttributeScalingFactor / 100) / 2);
        var skillMaximumBaseDamage =
            character.Level
            * (MaxBaseDamageFactor / 10)
            * (scalingAttributeAmount * (BaseDamageAttributeScalingFactor / 100) / 2);

        return new DamageRange(skillMinimumBaseDamage, skillMaximumBaseDamage);
    }

    public int GetSkillDamage(Weapon? weapon, Character character)
    {
        var weaponDamage =
            weapon != null ? RNG.GetIntInRange(weapon.MinDamage, weapon.MaxDamage) : 0;
        var skillWeaponDamageComponent = weaponDamage * (WeaponDamagePercentage / 100);

        var skillBaseDamageRange = GetSkillBaseDamageRange(character);
        var skillBaseDamageComponent = RNG.GetIntInRange(
            skillBaseDamageRange.MinDamage,
            skillBaseDamageRange.MaxDamage
        );

        return skillWeaponDamageComponent + skillBaseDamageComponent;
    }
}

public record DamageRange(int MinDamage, int MaxDamage);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SkillTargetType
{
    Unknown,
    Self,
    Friendly,
    Enemy,
    AllEnemies
}
