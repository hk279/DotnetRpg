using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Skills;

public class SkillInstance : CharacterSpecificEntity
{
    public SkillInstance() { }

    public SkillInstance(int characterId, Skill skill, int remainingCooldown) : base(characterId)
    {
        Skill = skill;
        RemainingCooldown = remainingCooldown;
    }

    public Skill Skill { get; set; } = null!;
    public int RemainingCooldown { get; set; }

    public void ApplyCooldown()
    {
        RemainingCooldown = Skill.Cooldown;
    }
}
