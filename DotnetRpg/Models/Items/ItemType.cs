using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Items;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemType
{
    Unknown = 0,
    Weapon = 1,
    ArmorPiece = 2
}