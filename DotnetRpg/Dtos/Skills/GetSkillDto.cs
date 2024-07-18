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
    GetStatusEffectDto? StatusEffect = null);

// TODO: Add FromSkill -method