using DotnetRpg.Models.Fights;

namespace DotnetRpg.Dtos.Fights;

public class PlayerActionResultDto
{
    public required ActionResultDto PlayerAction { get; set; }
    public List<ActionResultDto> EnemyActions { get; set; } = [];
    public FightStatus FightStatus { get; set; }
}