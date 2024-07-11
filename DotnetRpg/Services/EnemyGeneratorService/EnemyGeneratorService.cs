using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Items;

namespace DotnetRpg.Services.EnemyGeneratorService;

public class EnemyGeneratorService : IEnemyGeneratorService
{
    private readonly List<EnemyTemplate> _singleEnemyTemplates =
    [
        new EnemyTemplate(
            "Grizzly Bear",
            EnemyType.Single,
            CharacterClass.Warrior,
            new Weapon { Name = "Claw", IsEquipped = true }
        ),
        new EnemyTemplate(
            "Deranged Knight",
            EnemyType.Single,
            CharacterClass.Warrior,
            new Weapon { Name = "Longsword", IsEquipped = true }
        ),
        new EnemyTemplate(
            "Rogue Wizard",
            EnemyType.Single,
            CharacterClass.Mage,
            new Weapon { Name = "Ornate Staff", IsEquipped = true }
        ),
        new EnemyTemplate(
            "Cultist Shaman",
            EnemyType.Single,
            CharacterClass.Priest,
            new Weapon { Name = "Sacrificial Knife", IsEquipped = true }
        )
    ];

    // TODO: Implement multi-enemy templates. Should be weaker than single enemies.
    private readonly List<List<EnemyTemplate>> _multiEnemyTemplates = []; 

    public List<Character> GetEnemies(int playerCharacterLevel)
    {
        // 20% probability to get multi-enemy combat
        if (RNG.GetBoolean(0.2))
        {
            // TODO: Implement multi-enemy groups
        }

        var enemyGenerationData = RNG.PickRandom(_singleEnemyTemplates);
        var singleEnemy = GenerateSingleEnemyCharacter(playerCharacterLevel, enemyGenerationData);

        return [singleEnemy];
    }

    private static Character GenerateSingleEnemyCharacter(
        int playerCharacterLevel,
        EnemyTemplate data
    )
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

        var enemyWeaponDamageBase = Math.Max(RNG.GetIntInRange(enemyLevel - 2, enemyLevel + 2), 1);
        data.Weapon.MinDamage = enemyWeaponDamageBase * 2;
        data.Weapon.MaxDamage = enemyWeaponDamageBase * 3;
        data.Weapon.IsEquipped = true;

        var enemy = new Character
        {
            Name = data.Name,
            Class = data.EnemyClass,
            Level = enemyLevel,
            IsPlayerCharacter = false,
            Strength = strength,
            Intelligence = intelligence,
            Stamina = stamina,
            Spirit = spirit,
            BaseArmor = armor,
            BaseResistance = resistance,
            Inventory = [data.Weapon]
        };

        enemy.CurrentEnergy = enemy.GetMaxEnergy();
        enemy.CurrentHitPoints = enemy.GetMaxHitPoints();

        return enemy;
    }

    /// <summary>
    /// 20% probability of an enemy with -1 level compared to the player character.
    /// 20% probability of an enemy with +1 level compared to the player character.
    /// </summary>
    private static int GetEnemyLevel(int playerCharacterLevel)
    {
        if (RNG.GetBoolean(0.2))
        {
            var enemyLevel = playerCharacterLevel - 1;
            return enemyLevel < 1 ? 1 : enemyLevel;
        }

        if (RNG.GetBoolean(0.2))
        {
            return playerCharacterLevel + 1;
        }

        return playerCharacterLevel;
    }

    private record EnemyTemplate(
        string Name,
        EnemyType EnemyType,
        CharacterClass EnemyClass,
        Weapon Weapon
    );

    private enum EnemyType
    {
        Unknown,
        Multi,
        Single,
        Boss
    }
}
