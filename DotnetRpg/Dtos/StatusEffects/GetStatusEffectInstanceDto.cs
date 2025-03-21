using DotnetRpg.Models.StatusEffects;

namespace DotnetRpg.Dtos.StatusEffects;

public record GetStatusEffectInstanceDto(int RemainingDuration, GetStatusEffectDto StatusEffect)
{
    public static GetStatusEffectInstanceDto FromStatusEffectInstance(StatusEffectInstance statusEffectInstance) =>
        new(statusEffectInstance.RemainingDuration,
            GetStatusEffectDto.FromStatusEffect(statusEffectInstance.StatusEffect));
}
