namespace dotnet_rpg.Models;

public class Consumable : Item
{
    public SkillTargetType TargetType { get; set; }
    public int Healing { get; set; }
    public int Damage { get; set; }
}
