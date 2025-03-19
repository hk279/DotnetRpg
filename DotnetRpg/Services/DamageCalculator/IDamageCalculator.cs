using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Fights;
using DotnetRpg.Models.Skills;

namespace DotnetRpg.Services.DamageCalculator;

public interface IDamageCalculator
{
    /// <summary>
    /// A skill has weapon and base damage components. Both components are scaled by character attributes independently.
    /// Damage is scaled down by the defender's armor / resistance.
    /// </summary>
    DamageInstance CalculateSkillDamage(Skill skill, Character attacker, Character defender);

    DamageInstance CalculateWeaponDamage(Character attacker, Character defender);

    double CalculateWeakHitChance(Character attacker);

    double CalculateCriticalHitChance(Character attacker);

    (int totalDamage, HitType hitType) ApplyWeakOrCriticalHitModifier(int damage, Character attacker);
}