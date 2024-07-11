using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.StatusEffects;

public class StatusEffectInstance : BaseEntity
{
    public StatusEffectInstance() { }

    public StatusEffectInstance(int userId, StatusEffect statusEffect, int remainingDuration): base(userId)
    {
        StatusEffect = statusEffect;
        RemainingDuration = remainingDuration;
    }
    
    public StatusEffect StatusEffect { get; set; } = null!;
    public Character Character { get; set; } = null!;
    public int RemainingDuration { get; set; }
}
