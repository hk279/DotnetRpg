namespace DotnetRpg.Models;

public class StatusEffectInstance : BaseEntity
{
    public StatusEffectInstance() { }

    public StatusEffectInstance(StatusEffect statusEffect, int remainingDuration)
    {
        StatusEffect = statusEffect;
        RemainingDuration = remainingDuration;
    }
    
    public StatusEffect StatusEffect { get; set; } = null!;
    public Character Character { get; set; } = null!;
    public int RemainingDuration { get; set; }
}
