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

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var allCharacters = await _context.Characters
            .Include(c => c.Skills)
            .Include(c => c.Weapon)
            .Where(c => c.User.Id == GetUserId())
            .Select(c => _autoMapper.Map<GetCharacterDto>(c))
            .ToListAsync();
        return new ServiceResponse<List<GetCharacterDto>> { Data = allCharacters };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        var character = await _context.Characters
            .Include(c => c.Skills)
            .Include(c => c.Weapon)
            .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

        if (character == null)
        {
            response.Success = false;
            response.Message = "Character not found";
        }

        response.Data = _autoMapper.Map<GetCharacterDto>(character);

        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        // TODO: Validation - existing name not allowed

        var characterToAdd = _autoMapper.Map<Character>(newCharacter);
        characterToAdd.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        // TODO: Return error response if user not found

        _context.Characters.Add(characterToAdd);
        await _context.SaveChangesAsync();

        var allCharacters = await _context.Characters
            .Include(c => c.Skills)
            .Include(c => c.Weapon)
            .Where(c => c.User.Id == GetUserId())
            .Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToListAsync();
        return new ServiceResponse<List<GetCharacterDto>> { Data = allCharacters };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var characterToUpdate = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id && c.User.Id == GetUserId());

            if (characterToUpdate == null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

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
            var characterToRemove = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

            if (characterToRemove == null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            _context.Characters.Remove(characterToRemove);
            await _context.SaveChangesAsync();
            response.Data = _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .Where(c => c.User.Id == GetUserId())
                .Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToList();

        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());

            if (character == null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

            if (skill == null)
            {
                response.Success = false;
                response.Message = "Skill not found";
                return response;
            }

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
        return int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

}

