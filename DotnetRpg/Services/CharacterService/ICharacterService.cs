using DotnetRpg.Dtos.Characters;

namespace DotnetRpg.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<GetCharacterListingDto>> GetAllPlayerCharacters();
        Task<GetCharacterDto> GetCharacterById(int id);
        Task<List<GetCharacterDto>> GetEnemies(int characterId);
        Task AddCharacter(AddCharacterDto newCharacter);
        Task DeleteCharacter(int id);
    }
}
