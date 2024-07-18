namespace DotnetRpg.Dtos.Skills;

public record GetSkillInstanceDto(int RemainingCooldown, GetSkillDto Skill)
{
    // TODO: Add FromSkillInstance -method
}

