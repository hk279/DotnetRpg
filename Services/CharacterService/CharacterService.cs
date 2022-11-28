using dotnet_rpg.Dtos.Character;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _autoMapper;
        private readonly DataContext _context;

        private static List<Character> characters = new List<Character> {
            new Character() { Id = 1, Name = "Grommash", Class = CharacterClass.Warrior },
            new Character() { Id = 2, Name = "Jaina", Class = CharacterClass.Mage },
            new Character() { Id = 3, Name = "Valeera", Class = CharacterClass.Rogue  },
            new Character() { Id = 4, Name = "Anduin", Class = CharacterClass.Priest }
        };

        public CharacterService(IMapper autoMapper, DataContext context)
        {
            _autoMapper = autoMapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var allCharacters = await _context.Characters.Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToListAsync();
            return new ServiceResponse<List<GetCharacterDto>> { Data = allCharacters };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            return new ServiceResponse<GetCharacterDto> { Data = _autoMapper.Map<GetCharacterDto>(character) };
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var characterToAdd = _autoMapper.Map<Character>(newCharacter);
            _context.Characters.Add(characterToAdd);
            await _context.SaveChangesAsync();

            var allCharacters = await _context.Characters.Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToListAsync();
            return new ServiceResponse<List<GetCharacterDto>> { Data = allCharacters };
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = new ServiceResponse<GetCharacterDto>();

            try
            {
                var characterToUpdate = await _context.Characters.FirstAsync(c => c.Id == updatedCharacter.Id);

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
                var characterToRemove = await _context.Characters.FirstAsync(c => c.Id == id);

                _context.Characters.Remove(characterToRemove);
                await _context.SaveChangesAsync();

                response.Data = _context.Characters.Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}