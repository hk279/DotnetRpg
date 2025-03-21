using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Items;

public abstract class Item
{
    protected Item() { }
    
    protected Item(
        int characterId,
        string name,
        ItemType type,
        int level,
        ItemRarity rarity,
        string description,
        int weight,
        int value,
        bool isEquipped,
        Attributes attributes)
    {
        CharacterId = characterId;
        Name = name;
        Type = type;
        Level = level;
        Rarity = rarity;
        Description = description;
        Weight = weight;
        Value = value;
        IsEquipped = isEquipped;
        Strength = attributes.Strength;
        Intelligence = attributes.Intelligence;
        Stamina = attributes.Stamina;
        Spirit = attributes.Spirit;
    }

    public int Id { get; set; }
    public Character Character { get; set; } = null!;
    public int CharacterId { get; set; }
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
