using DotnetRpg.Dtos.Character;

namespace DotnetRpg.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<GetCharacterListingDto>> GetAllCharacters();
        Task<GetCharacterDto> GetCharacterById(int id);
        Task<List<GetCharacterDto>> GetEnemies(int characterId);
        Task AddCharacter(AddCharacterDto newCharacter);
        Task DeleteCharacter(int id);
    }
}
