namespace dotnet_rpg.Dtos.Fight;

public class BeginFightResultDto
{
    public int Id { get; init; }
    public int PlayerCharacterId { get; init; }
    public int EnemyCharacterId { get; init; }
}
