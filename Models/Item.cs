namespace dotnet_rpg.Models;

public abstract class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ItemType Type { get; set; }
    public int Level { get; set; } = 1;
    public ItemRarity Rarity { get; set; } = ItemRarity.Common;
    public string Description { get; set; } = string.Empty;
    public int Weight { get; set; } = 1;
    public int Value { get; set; } = 1;
}

public enum ItemRarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4
}

public enum ItemType
{
    Consumable = 1,
    Gear = 2
}
