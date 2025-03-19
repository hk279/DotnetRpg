using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Items;

public class Weapon : Item
{
    public Weapon() { }

    public Weapon(string name, string description = "")
    {
        Name = name;
        Description = description;
    }

    public Weapon(
        int characterId,
        string name,
        int level,
        ItemRarity rarity,
        int weight,
        int value,
        Attributes attributes,
        int minDamage,
        int maxDamage,
        string description = ""
        ) : base(characterId, name, ItemType.Weapon, level, rarity, description, weight, value, false, attributes)
    {
        MinDamage = minDamage;
        MaxDamage = maxDamage;
    }

    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
}
