using System.Text.Json.Serialization;

namespace DotnetRpg.Models;

public class ArmorPiece : Item
{
    public required ArmorSlot Slot { get; set; }
    public int Armor { get; set; }
    public int Resistance { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ArmorSlot
{
    Unknown = 0,
    Head = 1,
    Chest = 2,
    Hands = 3,
    Legs = 4,
    Feet = 5
}
