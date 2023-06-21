namespace dotnet_rpg.Dtos.Fight;

public class BeginFightDto
{
    public int PlayerCharacterId { get; set; }
    public List<int> EnemyIds { get; set; } = null!;
}
