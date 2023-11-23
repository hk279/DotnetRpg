namespace dotnet_rpg.Dtos.StatusEffect;

public class GetStatusEffectInstanceDto
{
    public int RemainingDuration { get; set; }
    public GetStatusEffectDto StatusEffect { get; set; } = null!;
}
