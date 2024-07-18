using DotnetRpg.Models.StatusEffects;

namespace DotnetRpg.Dtos.StatusEffects;

public record GetStatusEffectDto(
    string Name,
    int Duration,
    StatusEffectType Type,
    //int DamagePerTurnFactor,
    //int HealingPerTurnFactor,
    int IncreasedDamagePercentage,
    int DecreasedDamagePercentage,
    int IncreasedDamageTakenPercentage,
    int DecreasedDamageTakenPercentage,
    bool IsStunned,
    int IncreasedStrengthPercentage,
    int ReducedStrengthPercentage,
    int IncreasedIntelligencePercentage,
    int ReducedIntelligencePercentage,
    int IncreasedSpiritPercentage,
    int ReducedSpiritPercentage,
    int IncreasedArmorPercentage,
    int ReducedArmorPercentage,
    int IncreasedResistancePercentage,
    int ReducedResistancePercentage
)
{
    public static GetStatusEffectDto FromStatusEffect(StatusEffect statusEffect) =>
        new(
            statusEffect.Name,
            statusEffect.Duration,
            statusEffect.Type,
            statusEffect.IncreasedDamagePercentage,
            statusEffect.DecreasedDamagePercentage,
            statusEffect.IncreasedDamageTakenPercentage,
            statusEffect.DecreasedDamageTakenPercentage,
            statusEffect.IsStunned,
            statusEffect.IncreasedStrengthPercentage,
            statusEffect.ReducedStrengthPercentage,
            statusEffect.IncreasedIntelligencePercentage,
            statusEffect.ReducedIntelligencePercentage,
            statusEffect.IncreasedSpiritPercentage,
            statusEffect.ReducedSpiritPercentage,
            statusEffect.IncreasedArmorPercentage,
            statusEffect.ReducedArmorPercentage,
            statusEffect.IncreasedResistancePercentage,
            statusEffect.ReducedResistancePercentage
        );
}
