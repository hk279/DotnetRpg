namespace DotnetRpg.Dtos.Fight;

public class PlayerActionDto
{
    public int FightId { get; set; }
    public int PlayerCharacterId { get; set; }
    public int? TargetCharacterId { get; set; }
}

public class PlayerSkillActionDto : PlayerActionDto
{
    public int SkillId { get; set; }
}
