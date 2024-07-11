using AutoMapper;
using DotnetRpg.Data;
using DotnetRpg.Dtos.Item;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Exceptions;
using DotnetRpg.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Services.InventoryService;

public class InventoryService : IInventoryService
{
    private const int FightRewardLevelVariance = 2;

    private readonly Dictionary<ArmorSlot, decimal> _armorSlotStatCoefficients =
        new()
        {
            { ArmorSlot.Chest, 1 },
            { ArmorSlot.Legs, 0.8m },
            { ArmorSlot.Head, 0.6m },
            { ArmorSlot.Hands, 0.4m },
            { ArmorSlot.Feet, 0.4m }
        };

    private readonly Dictionary<ItemRarity, decimal> _itemRarityStatCoefficients =
        new()
        {
            { ItemRarity.Common, 1 },
            { ItemRarity.Uncommon, 1.1m },
            { ItemRarity.Rare, 1.3m },
            { ItemRarity.Epic, 1.6m }
        };

    private readonly Dictionary<ItemRarity, decimal> _itemRarityDropRates =
        new()
        {
            { ItemRarity.Common, 0.8m },
            { ItemRarity.Uncommon, 0.12m },
            { ItemRarity.Rare, 0.06m },
            { ItemRarity.Epic, 0.02m }
        };

    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _autoMapper;

    public InventoryService(
        DataContext context,
        IHttpContextAccessor httpContextAccessor,
        IMapper autoMapper
    )
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _autoMapper = autoMapper;
    }

    public async Task<List<GetItemDto>> GetInventory(int characterId)
    {
        var character = await _context.Characters
                            .Include(c => c.Inventory)
                            .FirstOrDefaultAsync(c => c.Id == characterId) 
                        ?? throw new NotFoundException("Character not found");

        var response = character.Inventory
            .Select(item => _autoMapper.Map<GetItemDto>(item))
            .ToList();

        return response;
    }

    public async Task EquipItem(int characterId, int itemId)
    {
        var character = await _context.Characters
                            .Include(c => c.Inventory)
                            .FirstOrDefaultAsync(c => c.Id == characterId) 
                        ?? throw new NotFoundException("Character not found");

        var itemToEquip = character.Inventory.FirstOrDefault(item => item.Id == itemId)
                          ?? throw new NotFoundException("Item not found");
        var equippedItems = character.Inventory.Where(item => item.IsEquipped).ToList();

        Item? itemToReplace = itemToEquip switch
        {
            Weapon => equippedItems.OfType<Weapon>().FirstOrDefault(),
            ArmorPiece armorPieceToEquip => equippedItems.OfType<ArmorPiece>()
                .FirstOrDefault(i => i.Slot == armorPieceToEquip.Slot),
            _ => null
        };

        if (itemToReplace != null)
        {
            itemToReplace.IsEquipped = false;
        }

        itemToEquip.IsEquipped = true;

        await _context.SaveChangesAsync();
    }

    public Task UnequipItem(int characterId, int itemId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteItem(int characterId, int itemId)
    {
        throw new NotImplementedException();
    }

    public Item GenerateFightRewardGear(Character playerCharacter, List<Character> enemies)
    {
        var itemLevelBase = (int)Math.Floor(enemies.Average(c => c.Level));
        var itemLevelWithVariance = Math.Max(
            RNG.GetIntInRange(
                itemLevelBase - FightRewardLevelVariance,
                itemLevelBase + FightRewardLevelVariance
            ),
            1
        );

        // TODO: Create item in DB
        // TODO: Add item to character's inventory

        return GenerateGearPiece(itemLevelWithVariance, playerCharacter.Class);
    }

    private ArmorPiece GenerateGearPiece(int itemLevel, CharacterClass characterClass)
    {
        // TODO: Generated item should be relevant to the character class

        var allGearSlots = Enum.GetValues(typeof(ArmorSlot)).Cast<ArmorSlot>().ToList();
        var armorSlot = RNG.PickRandom(allGearSlots);
        var itemRarity = GetItemRarity();

        var baseArmor = itemLevel * 10 + (itemLevel * 3);
        var armor = (int)
            Math.Floor(
                baseArmor
                    * _itemRarityStatCoefficients[itemRarity]
                    * _armorSlotStatCoefficients[armorSlot]
            );

        var baseResistance = itemLevel * 8 + (itemLevel * 3);
        var resistance = (int)
            Math.Floor(
                baseResistance
                    * _itemRarityStatCoefficients[itemRarity]
                    * _armorSlotStatCoefficients[armorSlot]
            );

        
        var gearPiece = new ArmorPiece();

        return gearPiece;
    }

    private Weapon GenerateWeapon(int itemLevel, CharacterClass characterClass)
    {
        throw new NotImplementedException();
    }

    private ItemRarity GetItemRarity()
    {
        var totalProbability = 0m;
        foreach (var dropRate in _itemRarityDropRates.Values)
        {
            totalProbability += dropRate;
        }

        var randomValue = (decimal)new Random().NextDouble() * totalProbability;
        var accumulatedProbability = 0m;

        foreach (var kvp in _itemRarityDropRates)
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
