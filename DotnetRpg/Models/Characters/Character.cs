using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotnetRpg.Models.Fights;
using DotnetRpg.Models.Generic;
using DotnetRpg.Models.Items;
using DotnetRpg.Models.Skills;
using DotnetRpg.Models.StatusEffects;

namespace DotnetRpg.Models.Characters;

public class Character : UserSpecificEntity
{
    public Character() { }

    public Character(
        int userId,
        string name,
        string avatar,
        CharacterClass characterClass,
        bool isPlayerCharacter,
        int level,
        Attributes attributes,
        int baseArmor,
        int baseResistance
    ) : base(userId)
    {
        Name = name;
        Avatar = avatar;
        Class = characterClass;
        IsPlayerCharacter = isPlayerCharacter;
        Level = level;
        Strength = attributes.Strength;
        Intelligence = attributes.Intelligence;
        Stamina = attributes.Stamina;
        Spirit = attributes.Spirit;
        BaseArmor = baseArmor;
        BaseResistance = baseResistance;
    }

    [MaxLength(32)] public string Name { get; set; } = null!;
    public string Avatar { get; set; } = string.Empty;
    public bool IsPlayerCharacter { get; set; }

    public int Level { get; set; } = 1;
    public int Experience { get; set; }
    public int UnassignedAttributePoints { get; set; }

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Stamina { get; set; }
    public int Spirit { get; set; }

    public int BaseArmor { get; set; }
    public int BaseResistance { get; set; }

    public int CurrentHitPoints { get; set; }
    public int CurrentEnergy { get; set; }

    public List<StatusEffectInstance> StatusEffectInstances { get; set; } = [];

    public CharacterClass Class { get; set; }
    public List<SkillInstance> SkillInstances { get; set; } = [];
    public int InventorySize { get; set; } = 10;
    public List<Item> Inventory { get; set; } = [];
    public Fight? Fight { get; set; }


    [NotMapped]
    public bool IsAlive => CurrentHitPoints > 0;

    [NotMapped]
    public bool IsDead => CurrentHitPoints <= 0;

    /// <summary>
    /// - Increases physical damage
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
    /// - Increases magic damage
    /// - Reduces chance of weak hit
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
    /// - Increases max HP
    /// - Increases max energy
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
    /// - Increases chance of critical hit
    /// - Increases energy regeneration
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
        return GetStamina() * 20;
    }

    /// <summary>
    /// - Base energy regeneration is 5 per turn
    /// - Every 2 points of spirit increases the regeneration by 1
    /// </summary>
    public int GetEnergyRegeneration()
    {
        const int baseEnergyRegeneration = 5;
        return baseEnergyRegeneration + (int)Math.Floor((decimal)GetSpirit() / 2);
    }
}
