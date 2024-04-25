namespace DotnetRpg.Dtos.Skill;

public class GetSkillInstanceDto
{
    public int RemainingCooldown { get; set; }
    public GetSkillDto Skill { get; set; } = null!;
}
