namespace DotnetRpg.Dtos.StatusEffect;

public class GetStatusEffectDto
{
    public required string Name { get; set; }
    public int Duration { get; set; }
    public StatusEffectType Type { get; set; }

    public int DamagePerTurn { get; set; }
    public int HealingPerTurn { get; set; }

    public int IncreasedDamagePercentage { get; set; }
    public int DecreasedDamagePercentage { get; set; }

    public int IncreasedDamageTakenPercentage { get; set; }
    public int DecreasedDamageTakenPercentage { get; set; }

    public bool IsStunned { get; set; }

    public int ReducedStrengthPercentage { get; set; }
    public int ReducedIntelligencePercentage { get; set; }
    public int ReducedArmorPercentage { get; set; }
    public int ReducedResistancePercentage { get; set; }

    public int IncreasedStrengthPercentage { get; set; }
    public int IncreasedIntelligencePercentage { get; set; }
    public int IncreasedArmorPercentage { get; set; }
    public int IncreasedResistancePercentage { get; set; }
}
