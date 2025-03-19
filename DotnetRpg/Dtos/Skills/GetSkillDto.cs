using DotnetRpg.Dtos.StatusEffects;
using DotnetRpg.Models.Skills;

namespace DotnetRpg.Dtos.Skills;

public record GetSkillDto(
    int Id,
    string Name,
    string Description,
    DamageType DamageType,
    SkillTargetType TargetType,
    int Rank,
    int WeaponDamagePercentage,
    int MinBaseDamage,
    int MaxBaseDamage,
    int EnergyCost,
    int Cooldown,
    GetStatusEffectDto? StatusEffect = null)
{
    public static GetSkillDto FromSkill(Skill skill) =>
        new(skill.Id,
            skill.Name,
            skill.Description,
            skill.DamageType,
            skill.TargetType,
            skill.Rank,
            skill.WeaponDamagePercentage,
            skill.MinBaseDamageFactor,
            skill.MaxBaseDamageFactor,
            skill.EnergyCost,
            skill.Cooldown,
            skill.StatusEffect != null ? GetStatusEffectDto.FromStatusEffect(skill.StatusEffect) : null);
}