using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Items;

public class ArmorPiece : Item
{
    public ArmorPiece() { }
    
    public ArmorPiece(
        int characterId,
        string name,
        int weight,
        ItemRarity rarity,
        int value,
        int level,
        ArmorSlot slot,
        int armor,
        int resistance,
        Attributes attributes,
        string description = "")
        :base(characterId, name, ItemType.ArmorPiece, level, rarity, description, weight, value, false, attributes)
    {
        Slot = slot;
        Armor = armor;
        Resistance = resistance;
    }

    public ArmorSlot Slot { get; set; }
    public int Armor { get; set; }
    public int Resistance { get; set; }
}