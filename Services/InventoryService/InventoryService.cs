using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Item;
using dotnet_rpg.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.ItemService;

public class InventoryService : IInventoryService
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

    public async Task<ServiceResponse<List<GetItemDto>>> GetInventory(int characterId)
    {
        var response = new ServiceResponse<List<GetItemDto>>();

        var character =
            await _context.Characters
                .Include(c => c.Inventory)
                .FirstOrDefaultAsync(
                    c => c.Id == characterId && c.User != null && c.User.Id == GetUserId()
                ) ?? throw new NotFoundException("Character not found");

        response.Data = character.Inventory
            .Select(item => _autoMapper.Map<GetItemDto>(item))
            .ToList();

        return response;
    }

    public async Task EquipItem(int characterId, int itemId)
    {
        var character =
            await _context.Characters
                .Include(c => c.Inventory)
                .FirstOrDefaultAsync(
                    c => c.Id == characterId && c.User != null && c.User.Id == GetUserId()
                ) ?? throw new NotFoundException("Character not found");

        var itemToEquip =
            character.Inventory.FirstOrDefault(item => item.Id == itemId)
            ?? throw new NotFoundException("Item not found");
        var equippedItems = character.Inventory.Where(item => item.IsEquipped);

        Item? itemToReplace = null;

        if (itemToEquip is Weapon)
        {
            itemToReplace = equippedItems.OfType<Weapon>().FirstOrDefault();
        }

        if (itemToEquip is ArmorPiece armorPieceToEquip)
        {
            itemToReplace = equippedItems
                .OfType<ArmorPiece>()
                .FirstOrDefault(i => i.Slot == armorPieceToEquip.Slot);
        }

        if (itemToReplace != null)
        {
            itemToReplace.IsEquipped = false;
        }

        itemToEquip.IsEquipped = true;
    }

    public Task UnequipItem(int characterId, int itemId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteItem(int characterId, int itemId)
    {
        throw new NotImplementedException();
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
        // TODO: Generated item should be relevant to the character class

        var allGearSlots = Enum.GetValues(typeof(ArmorSlot)).Cast<ArmorSlot>().ToList();
        var armorSlot = RNG.PickRandom(allGearSlots);
        var itemRarity = GetItemRarity();

        var baseArmor = itemLevel * 10 + (itemLevel * 3);
        var armor = (int)
            Math.Floor(
                baseArmor
                    * ItemRarityStatCoefficients[itemRarity]
                    * ArmorSlotStatCoefficients[armorSlot]
            );

        var baseResistance = itemLevel * 8 + (itemLevel * 3);
        var resistance = (int)
            Math.Floor(
                baseResistance
                    * ItemRarityStatCoefficients[itemRarity]
                    * ArmorSlotStatCoefficients[armorSlot]
            );

        var gearPiece = new ArmorPiece()
        {
            Name = "Generated armor piece", // TODO: Add name generation
            Level = itemLevel,
            Rarity = itemRarity,
            Weight = 2, // TODO: Generate by armor slot with some variance
            Value = 10, // TODO: Generate by armor slot and rarity with some variance
            Slot = armorSlot,
            Armor = armor,
            Resistance = resistance,
        };

        return gearPiece;
    }

    private Weapon GenerateWeapon(int itemLevel, CharacterClass characterClass)
    {
        throw new NotImplementedException();
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

    // TODO: Handle the user ID check in EF query filter
    private int GetUserId()
    {
        var httpContext =
            _httpContextAccessor.HttpContext
            ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext));
        var userId =
            httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("User not identified");
        return int.Parse(userId);
    }
}
