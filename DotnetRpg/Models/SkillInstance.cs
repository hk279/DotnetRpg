namespace DotnetRpg.Models;

public class SkillInstance : BaseEntity
{
    public SkillInstance() { }

    public SkillInstance(Skill skill, int remainingCooldown)
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
