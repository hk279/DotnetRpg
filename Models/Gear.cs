namespace dotnet_rpg.Models;

public class Gear : Item
{
    public required GearSlot Slot { get; set; }
    public int Armor { get; set; }
    public int Resistance { get; set; }
    public int Strength { get; set; } = 0;
    public int Intelligence { get; set; } = 0;
    public int Stamina { get; set; } = 0;
    public int Spirit { get; set; } = 0;
}

public enum GearSlot
{
    Head = 1,
    Chest = 2,
    Hands = 3,
    Legs = 4,
    Feet = 5
}
