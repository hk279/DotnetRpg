using DotnetRpg.Dtos.Characters;

namespace DotnetRpg.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<CharacterListingDto>> GetAllPlayerCharacters();
        Task<CharacterDto> GetCharacterById(int id);
        Task<List<CharacterDto>> GetEnemies(int characterId);
        Task AddCharacter(AddCharacterDto addCharacterDto);
        Task DeleteCharacter(int id);
        Task AssignAttributePoints(AssignAttributePointsDto assignAttributePointsDto);
    }
}
