using DotnetRpg.Models.Characters;

namespace DotnetRpg.Services.EnemyGeneratorService;

public interface IEnemyGeneratorService
{
    List<Character> GetEnemies(Character playerCharacter);
}
