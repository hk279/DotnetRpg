namespace dotnet_rpg.Dtos.Fight;

public class AttackResultDto
{
    public string AttackerName { get; set; }
    public string DefenderName { get; set; }
    public int AttackerHitPoints { get; set; }
    public int DefenderHitPoints { get; set; }
    public int Damage { get; set; }
}
