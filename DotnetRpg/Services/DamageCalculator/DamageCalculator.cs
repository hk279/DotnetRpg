using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Fights;
using DotnetRpg.Models.Items;
using DotnetRpg.Models.Skills;

namespace DotnetRpg.Services.DamageCalculator;

public class DamageCalculator : IDamageCalculator
{
    public DamageInstance CalculateSkillDamage(Skill skill, Character attacker, Character defender)
    {
        var scalingAttributeAmount = skill.DamageType switch
        {
            DamageType.Physical => attacker.GetStrength(),
            DamageType.Magic => attacker.GetIntelligence(),
            _ => throw new ArgumentOutOfRangeException(nameof(DamageType), "Unknown damage type")
        };

        // Calculate skill weapon damage component
        var attackerWeapon = attacker.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);
        var attackerWeaponDamage =
            attackerWeapon != null
                ? RNG.GetIntInRange(attackerWeapon.MinDamage, attackerWeapon.MaxDamage)
                : 1;
        var weaponDamageComponent = attackerWeaponDamage * (skill.WeaponDamagePercentage / 100);

        // Calculate skill base damage component
        var baseDamage = RNG.GetIntInRange(skill.MinBaseDamageFactor, skill.MaxBaseDamageFactor);
        var baseDamageComponent = baseDamage + (scalingAttributeAmount * (skill.BaseDamageAttributeScalingFactor / 100) / 2);
        
        var skillDamage = weaponDamageComponent + baseDamageComponent;

        // Calculate damage reduction
        // (Armor / ([85 * Enemy_Level] + Armor + 20))
        const int attackerLevelMultiplier = 85;
        const int magicNumber = 20;

        decimal damageReductionCoefficient;

        switch (skill.DamageType)
        {
            case DamageType.Physical:
                var armor = (decimal)defender.GetArmor();
                damageReductionCoefficient =
                    Math.Round(armor / ((attackerLevelMultiplier * attacker.Level) + armor + magicNumber), 2);
                break;
            case DamageType.Magic:
                var resistance = (decimal)defender.GetResistance();
                damageReductionCoefficient =
                    Math.Round(resistance / ((attackerLevelMultiplier * attacker.Level) + resistance + magicNumber), 2);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skill.DamageType), "Invalid damage type");
        }

        var damage = (int)Math.Round(skillDamage * (1 - damageReductionCoefficient));
        var (totalDamage, hitType) = ApplyWeakOrCriticalHitModifier(damage, attacker);
        
        return new DamageInstance(totalDamage, skill.DamageType, hitType);
    }

    public DamageInstance CalculateWeaponDamage(Character attacker, Character defender)
    {
        // Weapons always deal physical damage
        var damageBonus = Math.Round((decimal)attacker.GetStrength() / 100, 2);
        var damageReduction = Math.Round((decimal)defender.GetArmor() / 100, 2);
        var attackerWeapon = attacker.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);

        var baseDamage = RNG.GetIntInRange(
            attackerWeapon?.MinDamage ?? 1,
            attackerWeapon?.MaxDamage ?? 1
        );
        var damageMultiplier = 1 + damageBonus - damageReduction;
        var damage = (int)Math.Round(baseDamage * damageMultiplier);
        var (totalDamage, hitType) = ApplyWeakOrCriticalHitModifier(damage, attacker);
        
        return new DamageInstance(totalDamage, DamageType.Physical, hitType);
    }

    /// <summary>
    /// Base weak hit chance is 10%. This is scaled down by attacker's INT.
    /// Every 1 point of INT will reduce the chance by 0,05%
    /// </summary>
    public double CalculateWeakHitChance(Character attacker)
    {
        const double baseChance = 0.1;
        var calculatedProbability = baseChance - attacker.GetIntelligence() * 0.0005;
        return Math.Max(calculatedProbability, 0);
    }

    /// <summary>
    /// Base critical hit chance is 5%. This is scaled up by attacker's SPI.
    /// Every 1 point of SPI will increase the chance by 0,05%
    /// </summary>
    public double CalculateCriticalHitChance(Character attacker)
    {
        const double baseChance = 0.05;
        var calculatedProbability = baseChance + attacker.GetSpirit() * 0.0005;
        return Math.Max(calculatedProbability, 0);
    }

    public (int totalDamage, HitType hitType) ApplyWeakOrCriticalHitModifier(int damage, Character attacker)
    {
        var isWeakHit = RNG.GetBoolean(CalculateWeakHitChance(attacker));
        var isCriticalHit = RNG.GetBoolean(CalculateCriticalHitChance(attacker));
        
        if (isWeakHit && !isCriticalHit) return ((int)(damage * 0.5m), HitType.Weak);
        if (isCriticalHit && !isWeakHit) return ((int)(damage * 1.5m), HitType.Critical);
        
        return (damage, HitType.Normal);
    }
}