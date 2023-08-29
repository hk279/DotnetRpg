namespace dotnet_rpg.Models;

// TODO: Inherit from Item
public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Damage { get; set; }
}
