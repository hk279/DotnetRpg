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
        // 20% probability to get multi-enemy combat
        if (RNG.GetBoolean(0.2))
        {
            // TODO: Implement multi-enemy groups
        }

        var enemyGenerationData = RNG.PickRandom(_singleEnemyTemplates);
        var singleEnemy = GenerateSingleEnemyCharacter(playerCharacterLevel, enemyGenerationData);

        return new List<Character> { singleEnemy };
    }

    private static Character GenerateSingleEnemyCharacter(int playerCharacterLevel, EnemyGenerationData data)
    {
        var enemyLevel = GetEnemyLevel(playerCharacterLevel);
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

    /// <summary>
    /// 20% probability for an enemy with -1 level compared to player character
    /// 20% probability for an enemy with +1 level compared to player character
    /// </summary>
    private static int GetEnemyLevel(int playerCharacterLevel)
    {
        if (RNG.GetBoolean(0.2))
        {
            var enemyLevel = playerCharacterLevel - 1;
            return enemyLevel < 1 ? 1 : enemyLevel;
        }

        if (RNG.GetBoolean(0.2)) return playerCharacterLevel + 1;

        return playerCharacterLevel;
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
