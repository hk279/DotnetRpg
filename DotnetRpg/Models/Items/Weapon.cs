namespace DotnetRpg.Models.Items;

public class Weapon : Item
{
    public Weapon() { }
    
    public Weapon(int userId,
        string name,
        int level,
        ItemRarity rarity,
        int weight,
        int value,
        Attributes attributes,
        int minDamage,
        int maxDamage,
        string description = ""
        ) : base(userId, name, ItemType.Weapon, level, rarity, description, weight, value, false, attributes)
    {
        MinDamage = minDamage;
        MaxDamage = maxDamage;
    }

    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
}