using dotnet_rpg.Data;

namespace dotnet_rpg.Services.ItemService;

public class ItemService : IItemService
{
    const int fightRewardLevelVariance = 2;

    private readonly Dictionary<ArmorSlot, decimal> ArmorSlotStatCoefficients =
        new()
        {
            { ArmorSlot.Chest, 1 },
            { ArmorSlot.Legs, 0.8m },
            { ArmorSlot.Head, 0.6m },
            { ArmorSlot.Hands, 0.4m },
            { ArmorSlot.Feet, 0.4m }
        };

    private readonly Dictionary<ItemRarity, decimal> ItemRarityStatCoefficients =
        new()
        {
            { ItemRarity.Common, 1 },
            { ItemRarity.Uncommon, 1.1m },
            { ItemRarity.Rare, 1.3m },
            { ItemRarity.Epic, 1.6m }
        };

    public readonly Dictionary<ItemRarity, decimal> ItemRarityDropRates =
        new()
        {
            { ItemRarity.Common, 0.8m },
            { ItemRarity.Uncommon, 0.12m },
            { ItemRarity.Rare, 0.06m },
            { ItemRarity.Epic, 0.02m }
        };

    private readonly DataContext _context;

    public ItemService(DataContext context)
    {
        _context = context;
    }

    public Item? GenerateFightRewardGear(Character playerCharacter, List<Character> enemies)
    {
        var itemLevelBase = (int)Math.Floor(enemies.Average(c => c.Level));
        var itemLevelWithVariance = Math.Max(
            RNG.GetIntInRange(
                itemLevelBase - fightRewardLevelVariance,
                itemLevelBase + fightRewardLevelVariance
            ),
            1
        );

        // TODO: Create item in DB
        // TODO: Add item to character's inventory

        return GenerateGearPiece(itemLevelWithVariance, playerCharacter.Class);
    }

    private ArmorPiece GenerateGearPiece(int itemLevel, CharacterClass characterClass)
    {
        var allGearSlots = Enum.GetValues(typeof(ArmorSlot)).Cast<ArmorSlot>().ToList();
        var gearSlot = RNG.PickRandom(allGearSlots);
        var itemRarity = GetItemRarity();

        var baseArmor = itemLevel * 10 + (itemLevel * 3);
        var armor = (int)
            Math.Floor(
                baseArmor
                    * ItemRarityStatCoefficients[itemRarity]
                    * ArmorSlotStatCoefficients[gearSlot]
            );

        var baseResistance = itemLevel * 8 + (itemLevel * 3);
        var resistance = (int)
            Math.Floor(
                baseResistance
                    * ItemRarityStatCoefficients[itemRarity]
                    * ArmorSlotStatCoefficients[gearSlot]
            );

        var gearPiece = new ArmorPiece()
        {
            Name = "Generated armor piece", // TODO: Add name generation
            Level = itemLevel,
            Rarity = itemRarity,
            Weight = 2, // TODO: Generate by gear slot with some variance
            Value = 10, // TODO: Generate by gear slot and rarity with some variance
            Slot = gearSlot,
            Armor = armor,
            Resistance = resistance,
        };

        return gearPiece;
    }

    private ItemRarity GetItemRarity()
    {
        var totalProbability = 0m;
        foreach (var dropRate in ItemRarityDropRates.Values)
        {
            totalProbability += dropRate;
        }

        var randomValue = (decimal)new Random().NextDouble() * totalProbability;
        var accumulatedProbability = 0m;

        foreach (var kvp in ItemRarityDropRates)
        {
            accumulatedProbability += kvp.Value;
            if (randomValue <= accumulatedProbability)
            {
                return kvp.Key;
            }
        }

        return ItemRarity.Common;
    }
}
