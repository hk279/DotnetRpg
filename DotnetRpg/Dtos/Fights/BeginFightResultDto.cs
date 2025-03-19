namespace DotnetRpg.Dtos.Fights;

public class BeginFightResultDto
{
    public int FightId { get; init; }
    public int PlayerCharacterId { get; init; }
    public required List<int> EnemyCharacterIds { get; init; }
}
