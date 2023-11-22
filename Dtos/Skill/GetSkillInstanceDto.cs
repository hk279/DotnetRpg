namespace dotnet_rpg.Dtos.Skill;

public class GetSkillInstanceDto : GetSkillDto
{
    public int RemainingCooldown { get; set; }
}
