namespace dotnet_rpg.Models;

public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; }
    public ItemRarity Rarity { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Weight { get; set; }
    public int Value { get; set; }
}

public enum ItemRarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4
}
