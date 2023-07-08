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

    public CharacterService(IMapper autoMapper, DataContext context, IHttpContextAccessor httpContextAccessor)
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
            var character = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == id && c.User != null && c.User.Id == GetUserId())
                ?? throw new Exception("Character not found");

            response.Data = _autoMapper.Map<GetCharacterDto>(character);
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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User != null && c.User.Id == GetUserId())
            ?? throw new Exception("Character not found");

            var fightId = character.FightId ?? throw new Exception("Character is not in a fight");
            var enemies = await _context.Characters
                .Where(c => c.FightId == fightId && !c.IsPlayerCharacter)
                .Select(c => _autoMapper.Map<GetCharacterDto>(c))
                .ToListAsync();

            if (!enemies.Any()) throw new Exception("No enemies found");

            response.Data = enemies;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var response = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var totalAttributes = newCharacter.Strength + newCharacter.Intelligence + newCharacter.Stamina + newCharacter.Spirit;
            if (totalAttributes > 30)
            {
                response.Success = false;
                response.Message = "Allowed total attribute points exceeded";
            }

            var characterToAdd = _autoMapper.Map<Character>(newCharacter);
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId()) ?? throw new Exception("User not found");
            characterToAdd.User = currentUser;

            await _context.Characters.AddAsync(characterToAdd);
            await _context.SaveChangesAsync();

            var allCharacters = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .Where(c => c.User != null && c.User.Id == GetUserId())
                .Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToListAsync();

            response.Data = allCharacters;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    // Possibly redundant. Used for testing purposes for now. 
    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var characterToUpdate = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id && c.User != null && c.User.Id == GetUserId())
                ?? throw new Exception("Character not found");

            _autoMapper.Map(updatedCharacter, characterToUpdate);
            await _context.SaveChangesAsync();
            response.Data = _autoMapper.Map<GetCharacterDto>(characterToUpdate);
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
            var characterToRemove = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User != null && c.User.Id == GetUserId())
            ?? throw new Exception("Character not found");

            _context.Characters.Remove(characterToRemove);
            await _context.SaveChangesAsync();
            response.Data = _context.Characters
                .Where(c => c.User != null && c.User.Id == GetUserId())
                .Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    // TODO: Changes coming. Skills will be added through other events.
    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId())
                ?? throw new Exception("Character not found");

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId)
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
        var httpContext = _httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext));
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentNullException("No UserId");
        return int.Parse(userId);
    }
}