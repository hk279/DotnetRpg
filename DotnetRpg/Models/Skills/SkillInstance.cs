using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Skills;

public class SkillInstance : BaseEntity
{
    public SkillInstance() { }

    public SkillInstance(int userId, Skill skill, int remainingCooldown) : base(userId)
    {
        Skill = skill;
        RemainingCooldown = remainingCooldown;
    }

    public Skill Skill { get; set; } = null!;
    public Character Character { get; set; } = null!;
    public int RemainingCooldown { get; set; }

    public void ApplyCooldown()
    {
        RemainingCooldown = Skill.Cooldown;
    }
}
