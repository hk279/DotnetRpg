using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Item;

public class GetEquippedItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ItemRarity Rarity { get; set; } = ItemRarity.Common;

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Stamina { get; set; }
    public int Spirit { get; set; }
}
