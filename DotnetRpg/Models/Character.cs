using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetRpg.Models;

public class Character : BaseEntity
{
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public bool IsPlayerCharacter { get; set; } = true;

    public int Level { get; set; } = 1;
    public int Experience { get; set; }
    public int UnassignedAttributePoints { get; set; } = 0;

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Stamina { get; set; }
    public int Spirit { get; set; }

    public int BaseArmor { get; set; }
    public int BaseResistance { get; set; }

    public int CurrentHitPoints { get; set; }
    public int CurrentEnergy { get; set; }

    public List<StatusEffectInstance> StatusEffectInstances { get; set; } =
        new List<StatusEffectInstance>();

    public CharacterClass Class { get; set; }
    public List<SkillInstance> SkillInstances { get; set; } = new List<SkillInstance>();
    public int InventorySize { get; set; } = 10;
    public List<Item> Inventory { get; set; } = new List<Item>();
    public Fight? Fight { get; set; }


    [NotMapped]
    public bool IsAlive => CurrentHitPoints > 0;

    [NotMapped]
    public bool IsDead => CurrentHitPoints <= 0;

    /// <summary>
    /// Strength increases physical damage
    /// </summary>
    public int GetStrength()
    {
        var strengthFromItems = Inventory
            .Where(item => item.IsEquipped)
            .Select(item => item.Strength)
            .Sum();
        return Strength + strengthFromItems;
    }

    /// <summary>
    /// Intelligence increases magic damage
    /// </summary>
    public int GetIntelligence()
    {
        var intelligenceFromItems = Inventory
            .Where(item => item.IsEquipped)
            .Select(item => item.Intelligence)
            .Sum();
        return Intelligence + intelligenceFromItems;
    }

    /// <summary>
    /// Stamina scales max HP
    /// </summary>
    public int GetStamina()
    {
        var staminaFromItems = Inventory
            .Where(item => item.IsEquipped)
            .Select(item => item.Stamina)
            .Sum();
        return Stamina + staminaFromItems;
    }

    /// <summary>
    /// Spirit scales max energy and energy regeneration
    /// </summary>
    public int GetSpirit()
    {
        var spiritFromItems = Inventory
            .Where(item => item.IsEquipped)
            .Select(item => item.Spirit)
            .Sum();
        return Spirit + spiritFromItems;
    }

    /// <summary>
    /// Armor provides mitigation against physical damage
    /// </summary>
    public int GetArmor()
    {
        var armorFromItems = Inventory
            .OfType<ArmorPiece>()
            .Where(item => item.IsEquipped)
            .Select(item => item.Armor)
            .Sum();
        return BaseArmor + armorFromItems;
    }

    /// <summary>
    /// Resistance provides mitigation against magic damage
    /// </summary>
    public int GetResistance()
    {
        var resistanceFromItems = Inventory
            .OfType<ArmorPiece>()
            .Where(item => item.IsEquipped)
            .Select(item => item.Resistance)
            .Sum();
        return BaseResistance + resistanceFromItems;
    }

    public int GetMaxHitPoints()
    {
        return GetStamina() * 20;
    }

    public int GetMaxEnergy()
    {
        return GetSpirit() * 20;
    }
}
