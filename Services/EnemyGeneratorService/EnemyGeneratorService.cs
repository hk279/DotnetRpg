namespace dotnet_rpg.Services.EnemyGeneratorService;

public class EnemyGeneratorService : IEnemyGeneratorService
{
    private readonly List<EnemyGenerationData> _singleEnemyTemplates = new()
    {
        new EnemyGenerationData("Grizzly Bear", EnemyType.Single, CharacterClass.Warrior),
        new EnemyGenerationData("Deranged Knight", EnemyType.Single, CharacterClass.Warrior),
        new EnemyGenerationData("Rogue Wizard", EnemyType.Single, CharacterClass.Mage),
        new EnemyGenerationData("Cultist Shaman", EnemyType.Single, CharacterClass.Priest),
    };

    private readonly List<List<EnemyGenerationData>> _multiEnemyTemplates = new()
    {
        new List<EnemyGenerationData>() {
            new EnemyGenerationData("Wild Boar", EnemyType.Multi, CharacterClass.Warrior),
            new EnemyGenerationData("Wild Boar", EnemyType.Multi, CharacterClass.Warrior),
            new EnemyGenerationData("Rabid Wild Boar", EnemyType.Multi, CharacterClass.Warrior)
        },

        new List<EnemyGenerationData>()
        {
            new EnemyGenerationData("Wolf", EnemyType.Multi, CharacterClass.Warrior),
            new EnemyGenerationData("Wolf", EnemyType.Multi, CharacterClass.Warrior),
            new EnemyGenerationData("Alpha Wolf", EnemyType.Multi, CharacterClass.Warrior)
        },

        new List<EnemyGenerationData>()
        {
            new EnemyGenerationData("Cultist Novice", EnemyType.Multi, CharacterClass.Priest),
            new EnemyGenerationData("Cultist Novice", EnemyType.Multi, CharacterClass.Warrior)
        },
    };

    public List<Character> GetEnemies(int playerCharacterLevel)
    {
        var random = new Random();

        // 20% probability to get multi-enemy combat
        if (random.Next(100) < 20)
        {
            // TODO: Implement multi-enemy groups
        }

        var index = random.Next(0, _singleEnemyTemplates.Count + 1);
        var singleEnemy = GenerateSingleEnemyCharacter(playerCharacterLevel, _singleEnemyTemplates[index], random);

        return new List<Character> { singleEnemy };
    }

    private static Character GenerateSingleEnemyCharacter(int playerCharacterLevel, EnemyGenerationData data, Random random)
    {
        var enemyLevel = GetEnemyLevel(playerCharacterLevel, random);
        var baseAttributeValue = 5;

        var strength = data.EnemyClass switch
        {
            CharacterClass.Warrior => baseAttributeValue + enemyLevel * 5,
            CharacterClass.Mage => baseAttributeValue + enemyLevel * 2,
            CharacterClass.Priest => baseAttributeValue + enemyLevel * 3,
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        var intelligence = data.EnemyClass switch
        {
            CharacterClass.Warrior => baseAttributeValue + enemyLevel * 2,
            CharacterClass.Mage => baseAttributeValue + enemyLevel * 5,
            CharacterClass.Priest => baseAttributeValue + enemyLevel * 3,
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        var stamina = data.EnemyClass switch
        {
            CharacterClass.Warrior => baseAttributeValue + enemyLevel * 4,
            CharacterClass.Mage => baseAttributeValue + enemyLevel * 3,
            CharacterClass.Priest => baseAttributeValue + enemyLevel * 4,
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        var spirit = data.EnemyClass switch
        {
            CharacterClass.Warrior => baseAttributeValue + enemyLevel * 3,
            CharacterClass.Mage => baseAttributeValue + enemyLevel * 3,
            CharacterClass.Priest => baseAttributeValue + enemyLevel * 5,
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        var armor = data.EnemyClass switch
        {
            CharacterClass.Warrior => baseAttributeValue + enemyLevel * 10,
            CharacterClass.Mage => baseAttributeValue + enemyLevel * 5,
            CharacterClass.Priest => baseAttributeValue + enemyLevel * 8,
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        var resistance = data.EnemyClass switch
        {
            CharacterClass.Warrior => baseAttributeValue + enemyLevel * 5,
            CharacterClass.Mage => baseAttributeValue + enemyLevel * 10,
            CharacterClass.Priest => baseAttributeValue + enemyLevel * 8,
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        return new Character(stamina, spirit)
        {
            Name = data.Name,
            Class = data.EnemyClass,
            Level = enemyLevel,
            IsPlayerCharacter = false,
            Strength = strength,
            Intelligence = intelligence,
            Armor = armor,
            Resistance = resistance
        };
    }

    private static int GetEnemyLevel(int playerCharacterLevel, Random random)
    {
        var randomNumber = random.Next(100);
        var enemyLevel = randomNumber switch
        {
            < 20 => playerCharacterLevel - 1, // 20% chance to be one level lower
            > 80 => playerCharacterLevel + 1, // 20% chance to be one lever higher
            _ => playerCharacterLevel
        };

        if (enemyLevel == 0) enemyLevel = 1;

        return enemyLevel;
    }

    private record EnemyGenerationData(string Name, EnemyType EnemyType, CharacterClass EnemyClass);

    private enum EnemyType
    {
        None,
        Multi,
        Single,
        Boss
    }
}
