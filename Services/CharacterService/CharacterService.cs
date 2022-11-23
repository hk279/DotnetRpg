using dotnet_rpg.Dtos.Character;
using AutoMapper;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _autoMapper;

        private static List<Character> characters = new List<Character> {
            new Character() { Id = 1, Name = "Grommash", Class = CharacterClass.Warrior },
            new Character() { Id = 2, Name = "Jaina", Class = CharacterClass.Mage },
            new Character() { Id = 3, Name = "Valeera", Class = CharacterClass.Rogue  },
            new Character() { Id = 4, Name = "Anduin", Class = CharacterClass.Priest }
        };

        public CharacterService(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var characterToAdd = _autoMapper.Map<Character>(newCharacter);
            characterToAdd.Id = characters.Max(c => c.Id) + 1;
            characters.Add(characterToAdd);

            var data = characters.Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToList();
            return new ServiceResponse<List<GetCharacterDto>> { Data = data };
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var data = characters.Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToList();
            return new ServiceResponse<List<GetCharacterDto>> { Data = data };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = characters.FirstOrDefault(c => c.Id == id);
            return new ServiceResponse<GetCharacterDto> { Data = _autoMapper.Map<GetCharacterDto>(character) };
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = new ServiceResponse<GetCharacterDto>();

            try
            {
                var characterToUpdate = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                _autoMapper.Map(updatedCharacter, characterToUpdate);
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
                var characterToRemove = characters.First(c => c.Id == id);
                characters.Remove(characterToRemove);
                response.Data = characters.Select(c => _autoMapper.Map<GetCharacterDto>(c)).ToList();
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