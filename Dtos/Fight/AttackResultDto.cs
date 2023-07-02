namespace dotnet_rpg.Dtos.Fight;

public class AttackResultDto
{
    public required string AttackerName { get; set; }
    public required string DefenderName { get; set; }
    public int AttackerHitPoints { get; set; }
    public int DefenderHitPoints { get; set; }
    public int Damage { get; set; }
    public FightStatus FightStatus { get; set; }
}
