using dotnet_rpg.Dtos.StatusEffect;

namespace dotnet_rpg.Dtos.Skill;

public class GetSkillDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; }
    public int Rank { get; set; }
    public int WeaponDamagePercentage { get; set; }
    public int MinBaseDamage { get; set; }
    public int MaxBaseDamage { get; set; }
    public int EnergyCost { get; set; }
    public int Cooldown { get; set; }
    public GetStatusEffectDto? StatusEffect { get; set; }
}
