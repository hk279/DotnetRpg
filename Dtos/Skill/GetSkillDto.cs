namespace dotnet_rpg.Dtos.Skill;

public class GetSkillDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Damage { get; set; }
    public int Healing { get; set; }
    public int EnergyCost { get; set; }
    public int Cooldown { get; set; }
    public SkillDamageType DamageType { get; set; }
    public SkillTargetType TargetType { get; set; }
}
