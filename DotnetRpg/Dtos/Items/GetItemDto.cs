using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Items;

public class GetItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ItemType Type { get; set; }
    public int Level { get; set; } = 1;
    public ItemRarity Rarity { get; set; } = ItemRarity.Common;
    public string Description { get; set; } = string.Empty;
    public int Weight { get; set; } = 1;
    public int Value { get; set; } = 1;
    public bool IsEquipped { get; set; } = false;

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Stamina { get; set; }
    public int Spirit { get; set; }

    // Weapon-specific properties
    public int? MinDamage { get; set; }
    public int? MaxDamage { get; set; }

    // Armor piece -specific properties
    public ArmorSlot? Slot { get; set; }
    public int? Armor { get; set; }
    public int? Resistance { get; set; }
}
