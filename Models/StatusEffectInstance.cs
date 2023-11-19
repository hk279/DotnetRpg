namespace dotnet_rpg.Models;

public class StatusEffectInstance
{
    public StatusEffectInstance() { }

    public StatusEffectInstance(StatusEffect statusEffect, int remainingDuration)
    {
        StatusEffect = statusEffect;
        RemainingDuration = remainingDuration;
    }

    public int Id { get; set; }
    public StatusEffect StatusEffect { get; set; } = null!;
    public Character Character { get; set; } = null!;
    public int RemainingDuration { get; set; }
}
