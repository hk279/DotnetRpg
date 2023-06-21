namespace dotnet_rpg.Dtos.Fight;

public class AttackResultDto
{
    public string AttackerName { get; set; } = null!;
    public string DefenderName { get; set; } = null!;
    public int AttackerHitPoints { get; set; }
    public int DefenderHitPoints { get; set; }
    public int Damage { get; set; }
    public FightStatus FightStatus { get; set; }
}
