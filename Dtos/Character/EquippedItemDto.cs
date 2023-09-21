namespace dotnet_rpg.Dtos.Character;

public class EquippedItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ItemRarity Rarity { get; set; } = ItemRarity.Common;
}
