namespace DotnetRpg.Models.StatusEffects;

/// <summary>
/// Template for character-specific status effect instances
/// </summary>
public class StatusEffect
{
    public StatusEffect() { }

    public StatusEffect(
        string name,
        int duration,
        StatusEffectType type,
        //int damagePerTurnFactor = 0,
        //int healingPerTurnFactor = 0,
        int increasedDamagePercentage = 0,
        int increasedDamageTakenPercentage = 0,
        bool isStunned = false,
        int increasedStrengthPercentage = 0,
        int reducedStrengthPercentage = 0,
        int increasedArmorPercentage = 0,
        int reducedArmorPercentage = 0
    )
    {
        Name = name;
        Duration = duration;
        Type = type;
        //DamagePerTurnFactor = damagePerTurnFactor;
        //HealingPerTurnFactor = healingPerTurnFactor;
        IncreasedDamagePercentage = increasedDamagePercentage;
        IncreasedDamageTakenPercentage = increasedDamageTakenPercentage;
        IsStunned = isStunned;
        IncreasedStrengthPercentage = increasedStrengthPercentage;
        ReducedStrengthPercentage = reducedStrengthPercentage;
        IncreasedArmorPercentage = increasedArmorPercentage;
        ReducedArmorPercentage = reducedArmorPercentage;

        // TODO: Add other effects to constructor
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Duration { get; set; }
    public StatusEffectType Type { get; set; }

    // TODO: Rethink how these are handled
    //public int DamagePerTurnFactor { get; set; }
    //public int HealingPerTurnFactor { get; set; }

    public int IncreasedDamagePercentage { get; set; }
    public int DecreasedDamagePercentage { get; set; }

    public int IncreasedDamageTakenPercentage { get; set; }
    public int DecreasedDamageTakenPercentage { get; set; }

    public bool IsStunned { get; set; }

    public int IncreasedStrengthPercentage { get; set; }
    public int ReducedStrengthPercentage { get; set; }
    public int IncreasedIntelligencePercentage { get; set; }
    public int ReducedIntelligencePercentage { get; set; }
    public int IncreasedSpiritPercentage { get; set; }
    public int ReducedSpiritPercentage { get; set; }

    public int IncreasedArmorPercentage { get; set; }
    public int ReducedArmorPercentage { get; set; }
    public int IncreasedResistancePercentage { get; set; }
    public int ReducedResistancePercentage { get; set; }

}