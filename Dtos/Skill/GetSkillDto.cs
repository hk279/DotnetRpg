namespace dotnet_rpg.Dtos.Skill;

public class GetSkillDto
{
    public required string Name { get; set; }
    public int Damage { get; set; }
    public SkillType DamageType { get; set; }
}
