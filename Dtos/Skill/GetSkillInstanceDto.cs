namespace dotnet_rpg.Dtos.Skill;

public class GetSkillInstanceDto
{
    public GetSkillDto Skill { get; set; } = null!;
    public int RemainingCooldown { get; set; }
}
