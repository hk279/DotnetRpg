using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.StatusEffects;

public class StatusEffectInstance : CharacterSpecificEntity
{
    public StatusEffectInstance() { }

    public StatusEffectInstance(int characterId, StatusEffect statusEffect, int remainingDuration): base(characterId)
    {
        StatusEffect = statusEffect;
        RemainingDuration = remainingDuration;
    }
    
    public StatusEffect StatusEffect { get; set; } = null!;
    public int RemainingDuration { get; set; }
}
