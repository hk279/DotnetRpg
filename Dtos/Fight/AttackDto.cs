namespace dotnet_rpg.Dtos.Fight;

public class AttackDto
{
    public int FightId { get; set; }
    public int AttackerId { get; set; }
    public int TargetId { get; set; }
}

public class SkillAttackDto : AttackDto
{
    public int SkillId { get; set; }
}
