using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Items;

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
