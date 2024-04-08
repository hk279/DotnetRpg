namespace DotnetRpg.Services.EnemyGeneratorService;

public interface IEnemyGeneratorService
{
    List<Character> GetEnemies(int playerCharacterLevel);
}
