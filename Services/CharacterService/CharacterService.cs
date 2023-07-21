using dotnet_rpg.Dtos.Character;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            .Include(c => c.Skills)
            .Include(c => c.Weapon)
            .Where(c => c.User != null && c.User.Id == GetUserId())
            .Select(c => _autoMapper.Map<GetCharacterListingDto>(c))
            .ToListAsync();

        return new ServiceResponse<List<GetCharacterListingDto>> { Data = allCharacters };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character =
                await _context.Characters
                    .Include(c => c.Skills)
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(
                        c => c.Id == id && c.User != null && c.User.Id == GetUserId()
                    ) ?? throw new Exception("Character not found");

            var dto = _autoMapper.Map<GetCharacterDto>(character);
            dto.NextLevelExperienceThreshold =
                LevelExperienceThresholds.AllThresholds.GetValueOrDefault(character.Level + 1);
            response.Data = dto;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetEnemies(int id)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var character =
                await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == id && c.User != null && c.User.Id == GetUserId()
                ) ?? throw new Exception("Character not found");

            var fightId = character.FightId ?? throw new Exception("Character is not in a fight");
            var enemies = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .Where(c => c.FightId == fightId && !c.IsPlayerCharacter)
                .Select(c => _autoMapper.Map<GetCharacterDto>(c))
                .ToListAsync();

            if (!enemies.Any())
                throw new Exception("No enemies found");

            response.Data = enemies;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(
        AddCharacterDto newCharacter
    )
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var totalAttributes =
                newCharacter.Strength
                + newCharacter.Intelligence
                + newCharacter.Stamina
                + newCharacter.Spirit;

            if (totalAttributes > 30)
                throw new Exception("Allowed total attribute points exceeded");

            var characterToAdd = _autoMapper.Map<Character>(newCharacter);
            var currentUser =
                await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId())
                ?? throw new Exception("User not found");

            characterToAdd.User = currentUser;
            AddStartingSkills(characterToAdd);
            AddStartingWeapon(characterToAdd);

            await _context.Characters.AddAsync(characterToAdd);
            await _context.SaveChangesAsync();

            var allCharacters = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .Where(c => c.User != null && c.User.Id == GetUserId())
                .Select(c => _autoMapper.Map<GetCharacterDto>(c))
                .ToListAsync();

            response.Data = allCharacters;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var characterToRemove =
                await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == id && c.User != null && c.User.Id == GetUserId()
                ) ?? throw new Exception("Character not found");

            _context.Characters.Remove(characterToRemove);
            await _context.SaveChangesAsync();
            response.Data = _context.Characters
                .Where(c => c.User != null && c.User.Id == GetUserId())
                .Select(c => _autoMapper.Map<GetCharacterDto>(c))
                .ToList();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    // TODO: Changes coming. Skills will be added through other events.
    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(
        AddCharacterSkillDto newCharacterSkill
    )
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character =
                await _context.Characters
                    .Include(c => c.Skills)
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(
                        c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId()
                    ) ?? throw new Exception("Character not found");

            var skill =
                await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId)
                ?? throw new Exception("Skill not found");

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();
            response.Data = _autoMapper.Map<GetCharacterDto>(character);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    private int GetUserId()
    {
        var httpContext =
            _httpContextAccessor.HttpContext
            ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext));
        var userId =
            httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new ArgumentNullException("No UserId");
        return int.Parse(userId);
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
                        Damage = 10,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 15,
                        Cooldown = 5
                    },
                    new Skill
                    {
                        Name = "Rend",
                        Damage = 5,
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
                        Damage = 20,
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
                        Damage = 20,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    new Skill
                    {
                        Name = "Combustion",
                        Damage = 5,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 10,
                        Cooldown = 3
                    },
                    new Skill
                    {
                        Name = "Lightning Storm",
                        Damage = 10,
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
                        Damage = 20,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    new Skill
                    {
                        Name = "Cleansing Pain",
                        Damage = 5,
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
            CharacterClass.Warrior => new Weapon() { Name = "Training Shortsword", Damage = 5 },
            CharacterClass.Mage => new Weapon() { Name = "Handmade Dagger", Damage = 5 },
            CharacterClass.Priest => new Weapon() { Name = "Traveling Staff", Damage = 5 },
            _ => throw new ArgumentException("Invalid character class")
        };

        character.Weapon = weapon;
    }
}
