namespace dotnet_rpg.Dtos.Fight;

public class AttackDto
{
    public int FightId { get; set; }
    public int PlayerCharacterId { get; set; }
    public int EnemyCharacterId { get; set; }
}

public class SkillAttackDto : AttackDto
{
    public int SkillId { get; set; }
}
