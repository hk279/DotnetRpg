using dotnet_rpg.Dtos.Character;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using dotnet_rpg.Models.Exceptions;
using dotnet_rpg.Dtos.Item;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _autoMapper;
    private readonly DataContext _context;
    public readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(
        IMapper autoMapper,
        DataContext context,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _autoMapper = autoMapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<List<GetCharacterListingDto>>> GetAllCharacters()
    {
        var allCharacters = await _context.Characters
            .Where(c => c.User != null && c.User.Id == GetUserId())
            .Select(c => _autoMapper.Map<GetCharacterListingDto>(c))
            .ToListAsync();

        return new ServiceResponse<List<GetCharacterListingDto>> { Data = allCharacters };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        var character =
            await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Inventory)
                .FirstOrDefaultAsync(c => c.Id == id && c.User != null && c.User.Id == GetUserId())
            ?? throw new NotFoundException("Character not found");

        var dto = _autoMapper.Map<GetCharacterDto>(character);

        var currentLevelExperienceThreshold = LevelExperienceThresholds.AllThresholds
            .Single(t => t.Key == character.Level)
            .Value;
        var nextLevelExperienceThreshold =
            LevelExperienceThresholds.AllThresholds.GetValueOrDefault(character.Level + 1);

        dto.CurrentLevelTotalExperience =
            nextLevelExperienceThreshold - currentLevelExperienceThreshold;
        dto.ExperienceTowardsNextLevel = character.Experience - currentLevelExperienceThreshold;

        response.Data = dto;

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetEnemies(int characterId)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        var character =
            await _context.Characters.FirstOrDefaultAsync(
                c => c.Id == characterId && c.User != null && c.User.Id == GetUserId()
            ) ?? throw new NotFoundException("Character not found");

        var fightId =
            character.FightId ?? throw new BadRequestException("Character is not in a fight");
        var enemies = await _context.Characters
            .Include(c => c.Skills)
            .Include(c => c.Inventory)
            .Where(c => c.FightId == fightId && !c.IsPlayerCharacter)
            .Select(c => _autoMapper.Map<GetCharacterDto>(c))
            .ToListAsync();

        if (!enemies.Any())
        {
            throw new NotFoundException("No enemies found");
        }

        response.Data = enemies;

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(
        AddCharacterDto newCharacter
    )
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        var totalAttributes =
            newCharacter.Strength
            + newCharacter.Intelligence
            + newCharacter.Stamina
            + newCharacter.Spirit;

        if (totalAttributes > 30)
        {
            throw new BadRequestException("Allowed total attribute points exceeded");
        }

        var characterToAdd = _autoMapper.Map<Character>(newCharacter);
        var currentUser =
            await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId())
            ?? throw new NotFoundException("User not found");

        characterToAdd.User = currentUser;

        SetCurrentHitPointsAndEnergy(characterToAdd);
        AddStartingSkills(characterToAdd);
        AddStartingWeapon(characterToAdd);
        AddStartingGear(characterToAdd);

        await _context.Characters.AddAsync(characterToAdd);
        await _context.SaveChangesAsync();

        var allCharacters = await _context.Characters
            .Include(c => c.Skills)
            .Include(c => c.Inventory)
            .Where(c => c.User != null && c.User.Id == GetUserId())
            .Select(c => _autoMapper.Map<GetCharacterDto>(c))
            .ToListAsync();

        response.Data = allCharacters;

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        var characterToRemove =
            await _context.Characters.FirstOrDefaultAsync(
                c => c.Id == id && c.User != null && c.User.Id == GetUserId()
            ) ?? throw new NotFoundException("Character not found");

        _context.Characters.Remove(characterToRemove);

        await _context.SaveChangesAsync();
        response.Data = _context.Characters
            .Where(c => c.User != null && c.User.Id == GetUserId())
            .Select(c => _autoMapper.Map<GetCharacterDto>(c))
            .ToList();

        return response;
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

    private static void SetCurrentHitPointsAndEnergy(Character character)
    {
        character.CurrentHitPoints = character.GetMaxHitPoints();
        character.CurrentEnergy = character.GetMaxEnergy();
    }

    private static void AddStartingSkills(Character character)
    {
        var skills = character.Class switch
        {
            CharacterClass.Warrior
                => new[]
                {
                    new Skill
                    {
                        Name = "Charge",
                        MinDamage = 8,
                        MaxDamage = 12,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 15,
                        Cooldown = 5
                    },
                    new Skill
                    {
                        Name = "Rend",
                        MinDamage = 4,
                        MaxDamage = 6,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 10,
                        Cooldown = 5
                    },
                    new Skill
                    {
                        Name = "Enrage",
                        DamageType = DamageType.Physical,
                        TargetType = SkillTargetType.Self,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 10,
                        Cooldown = 10
                    },
                    new Skill
                    {
                        Name = "Skillful Strike",
                        MinDamage = 18,
                        MaxDamage = 22,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                },
            CharacterClass.Mage
                => new[]
                {
                    new Skill
                    {
                        Name = "Arcane Barrier",
                        DamageType = DamageType.Magic,
                        TargetType = SkillTargetType.Friendly,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 15,
                        Cooldown = 10
                    },
                    new Skill
                    {
                        Name = "Ice Lance",
                        MinDamage = 18,
                        MaxDamage = 22,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    new Skill
                    {
                        Name = "Combustion",
                        MinDamage = 4,
                        MaxDamage = 6,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 10,
                        Cooldown = 3
                    },
                    new Skill
                    {
                        Name = "Lightning Storm",
                        MinDamage = 4,
                        MaxDamage = 16,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 30,
                        Cooldown = 10
                    },
                },
            CharacterClass.Priest
                => new[]
                {
                    new Skill
                    {
                        Name = "Battle Meditation",
                        DamageType = DamageType.Magic,
                        TargetType = SkillTargetType.Self,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 10,
                        Cooldown = 10
                    },
                    new Skill
                    {
                        Name = "Miraclous Touch",
                        Healing = 20,
                        DamageType = DamageType.Magic,
                        TargetType = SkillTargetType.Friendly,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 15,
                        Cooldown = 3
                    },
                    new Skill
                    {
                        Name = "Holy Smite",
                        MinDamage = 18,
                        MaxDamage = 22,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    new Skill
                    {
                        Name = "Cleansing Pain",
                        MinDamage = 4,
                        MaxDamage = 6,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 10,
                        Cooldown = 3
                    }
                },
            _ => throw new ArgumentException("Invalid character class")
        };

        character.Skills.AddRange(skills);
    }

    private static void AddStartingWeapon(Character character)
    {
        var weapon = character.Class switch
        {
            CharacterClass.Warrior
                => new Weapon()
                {
                    Name = "Training Shortsword",
                    MinDamage = 4,
                    MaxDamage = 5
                },
            CharacterClass.Mage
                => new Weapon()
                {
                    Name = "Handmade Dagger",
                    MinDamage = 4,
                    MaxDamage = 5
                },
            CharacterClass.Priest
                => new Weapon()
                {
                    Name = "Traveling Staff",
                    MinDamage = 4,
                    MaxDamage = 5
                },
            _ => throw new ArgumentException("Invalid character class")
        };

        weapon.IsEquipped = true;

        character.Inventory.Add(weapon);
    }

    private static void AddStartingGear(Character character)
    {
        var armorPieces = character.Class switch
        {
            CharacterClass.Warrior
                => new List<ArmorPiece>
                {
                    new()
                    {
                        Name = "Worn Leather Training Cuirass",
                        Slot = ArmorSlot.Chest,
                        Armor = 10,
                        Resistance = 4,
                        Weight = 3
                    },
                    new()
                    {
                        Name = "Patched Leather Pants",
                        Slot = ArmorSlot.Legs,
                        Armor = 5,
                        Resistance = 2,
                        Weight = 2
                    },
                    new()
                    {
                        Name = "Mudworn Boots",
                        Slot = ArmorSlot.Feet,
                        Armor = 3,
                        Resistance = 1,
                        Weight = 1
                    },
                },
            CharacterClass.Mage
                => new List<ArmorPiece>
                {
                    new()
                    {
                        Name = "Worn Linen Tunic",
                        Slot = ArmorSlot.Chest,
                        Armor = 4,
                        Resistance = 4
                    },
                    new()
                    {
                        Name = "Linen Leggings",
                        Slot = ArmorSlot.Legs,
                        Armor = 2,
                        Resistance = 2
                    },
                    new()
                    {
                        Name = "Handmade Sandals",
                        Slot = ArmorSlot.Feet,
                        Armor = 1,
                        Resistance = 1
                    },
                },
            CharacterClass.Priest
                => new List<ArmorPiece>
                {
                    new()
                    {
                        Name = "Gray Novice Robe",
                        Slot = ArmorSlot.Chest,
                        Armor = 4,
                        Resistance = 4
                    },
                    new()
                    {
                        Name = "Sackcloth Pants",
                        Slot = ArmorSlot.Legs,
                        Armor = 2,
                        Resistance = 2
                    },
                    new()
                    {
                        Name = "Sturdy Footwraps",
                        Slot = ArmorSlot.Feet,
                        Armor = 1,
                        Resistance = 1
                    },
                },
            _ => throw new ArgumentException("Invalid character class")
        };

        foreach (var armorPiece in armorPieces)
        {
            armorPiece.IsEquipped = true;
        }

        character.Inventory.AddRange(armorPieces);
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
}
