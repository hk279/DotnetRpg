namespace dotnet_rpg.Models;

public class StatusEffect
{
    public StatusEffect() { }

    public StatusEffect(
        string name,
        int duration,
        int skillId,
        int damagePerTurn = 0,
        int increasedDamagePercentage = 0,
        int increasedDamageTakenPercentage = 0,
        bool isStunned = false,
        int reducedStrengthPercentage = 0,
        int reducedArmorPercentage = 0,
        int increasedStrengthPercentage = 0,
        int increasedArmorPercentage = 0
    )
    {
        Name = name;
        Duration = duration;
        SkillId = skillId;
        DamagePerTurn = damagePerTurn;
        IncreasedDamagePercentage = increasedDamagePercentage;
        IncreasedDamageTakenPercentage = increasedDamageTakenPercentage;
        IsStunned = isStunned;
        ReducedStrengthPercentage = reducedStrengthPercentage;
        ReducedArmorPercentage = reducedArmorPercentage;
        IncreasedStrengthPercentage = increasedStrengthPercentage;
        IncreasedArmorPercentage = increasedArmorPercentage;
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Duration { get; set; }

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

    public List<StatusEffectInstance> StatusEffectInstances { get; set; } =
        new List<StatusEffectInstance>();

    public Skill Skill { get; set; } = null!;
    public int SkillId { get; set; }
}
