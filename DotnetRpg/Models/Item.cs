using System.Text.Json.Serialization;

namespace DotnetRpg.Models;

public abstract class Item : BaseEntity
{
    public string Name { get; set; } = null!;
    public ItemType Type { get; set; }
    public int Level { get; set; } = 1;
    public ItemRarity Rarity { get; set; }
    public string Description { get; set; } = null!;
    public int Weight { get; set; } = 1;
    public int Value { get; set; } = 1;
    public bool IsEquipped { get; set; }

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Stamina { get; set; }
    public int Spirit { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemRarity
{
    Unknown = 0,
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemType
{
    Unknown = 0,
    Weapon = 1,
    ArmorPiece = 2
}
