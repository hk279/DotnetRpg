using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Items;

namespace DotnetRpg.Services.EnemyGeneratorService;

public class EnemyGeneratorService : IEnemyGeneratorService
{
    private const int SingleEnemyBaseAttributeAmount = 6;
    private const int SingleEnemyBaseMitigationAmount = 12;

    private readonly List<((int MinLevel, int MaxLevel) LevelRange, EnemyTemplate Enemy)> _singleEnemyTemplatesByLevelRange = new()
    {
        ((1, 3), new EnemyTemplate("Mutant Rat", CharacterClass.Warrior, new Weapon("Giant Teeth"))),
        ((1, 3), new EnemyTemplate("Zombified Peasant", CharacterClass.Warrior, new Weapon("Pitchfork"))),
        ((3, 5), new EnemyTemplate("Cultist Gatherer", CharacterClass.Warrior, new Weapon("Bone Knife"))),
        ((3, 5), new EnemyTemplate("Grizzly Bear", CharacterClass.Warrior, new Weapon("Claws"))),
        ((5, 7), new EnemyTemplate("Deranged Knight", CharacterClass.Warrior, new Weapon("Longsword"))),
        ((5, 7), new EnemyTemplate("Cultist Shaman", CharacterClass.Priest, new Weapon("Sacrificial Knife")))
    };

    // Multi-enemy fights can have 2 or 3 enemies
    private readonly List<((int MinLevel, int MaxLevel) LevelRange, List<EnemyTemplate> EnemyGroup)> _multiEnemyTemplatesByLevelRange = new()
    {
        (
            (1, 5),
            [
                new EnemyTemplate("Vulture", CharacterClass.Priest, new Weapon("Claws")),
                new EnemyTemplate("Vulture", CharacterClass.Priest, new Weapon("Claws")),
                new EnemyTemplate("Vulture", CharacterClass.Priest, new Weapon("Claws"))
            ]
        ),
        (
            (5, 10),
            [
                new EnemyTemplate("Highwayman", CharacterClass.Warrior, new Weapon("Club")),
                new EnemyTemplate("Highwayman", CharacterClass.Warrior, new Weapon("Dagger")),
                new EnemyTemplate("Highwayman", CharacterClass.Warrior, new Weapon("Crossbow"))
            ]
        )
    };

    public List<Character> GetEnemies(Character playerCharacter)
    {
        var isMultiEnemyFight = RNG.GetBoolean(0.2);

        if (isMultiEnemyFight)
        {
            var enemyGroupsInLevelRange = _multiEnemyTemplatesByLevelRange
                .Where(x => playerCharacter.Level >= x.LevelRange.MinLevel && playerCharacter.Level <= x.LevelRange.MaxLevel)
                .Select(x => x.EnemyGroup)
                .ToList();
            var enemyTemplates = RNG.PickRandom(enemyGroupsInLevelRange);
            var enemyCharacters = enemyTemplates.Select(t => GetMultiEnemyCharacter(playerCharacter, t, enemyTemplates.Count));

            return enemyCharacters.ToList();
        }

        var enemiesInLevelRange = _singleEnemyTemplatesByLevelRange
            .Where(x => playerCharacter.Level >= x.LevelRange.MinLevel && playerCharacter.Level <= x.LevelRange.MaxLevel)
            .Select(x => x.Enemy)
            .ToList();
        var enemyTemplate = RNG.PickRandom(enemiesInLevelRange);
        var enemyCharacter = GetSingleEnemyCharacter(playerCharacter, enemyTemplate);

        return [enemyCharacter];
    }

    private static Character GetSingleEnemyCharacter(Character playerCharacter, EnemyTemplate data)
    {
        var enemyLevel = GetEnemyLevel(playerCharacter.Level);

        var attributeCoefficients = data.EnemyClass switch
        {
            CharacterClass.Warrior => new AttributeCoefficients(5, 4, 2, 2, 10, 5),
            CharacterClass.Mage => new AttributeCoefficients(2, 2, 5, 4, 5, 10),
            CharacterClass.Priest => new AttributeCoefficients(3, 3, 3, 4, 7, 8),
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        if (data.Weapon is not null)
        {
            var enemyWeaponDamageBase = Math.Max(RNG.GetIntInRange(enemyLevel - 2, enemyLevel + 2), 1);

            data.Weapon.MinDamage = enemyWeaponDamageBase * 2;
            data.Weapon.MaxDamage = enemyWeaponDamageBase * 3;
            data.Weapon.IsEquipped = true;
        }

        var enemy = new Character
        {
            UserId = playerCharacter.UserId,
            Name = data.Name,
            Class = data.EnemyClass,
            Level = enemyLevel,
            IsPlayerCharacter = false,
            Strength = SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.StrengthCoefficient,
            Stamina = SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.StaminaCoefficient,
            Intelligence = SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.IntelligenceCoefficient,
            Spirit = SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.SpiritCoefficient,
            BaseArmor = SingleEnemyBaseMitigationAmount + enemyLevel * attributeCoefficients.ArmorCoefficient,
            BaseResistance = SingleEnemyBaseMitigationAmount + enemyLevel * attributeCoefficients.ResistanceCoefficient,
            Inventory = data.Weapon is not null ? [data.Weapon] : []
        };

        enemy.CurrentEnergy = enemy.GetMaxEnergy();
        enemy.CurrentHitPoints = enemy.GetMaxHitPoints();

        return enemy;
    }

    private static Character GetMultiEnemyCharacter(Character playerCharacter, EnemyTemplate data, int enemyCount)
    {
        var enemyLevel = GetEnemyLevel(playerCharacter.Level);

        var attributeCoefficients = data.EnemyClass switch
        {
            CharacterClass.Warrior => new AttributeCoefficients(5, 4, 2, 2, 10, 5),
            CharacterClass.Mage => new AttributeCoefficients(2, 2, 5, 4, 5, 10),
            CharacterClass.Priest => new AttributeCoefficients(3, 3, 3, 4, 7, 8),
            _ => throw new ArgumentException("Invalid enemy class"),
        };

        // TODO: Adjust weapon damage for multi enemy fights
        if (data.Weapon is not null)
        {
            var enemyWeaponDamageBase = Math.Max(RNG.GetIntInRange(enemyLevel - 2, enemyLevel + 2), 1);

            data.Weapon.MinDamage = enemyWeaponDamageBase * 2;
            data.Weapon.MaxDamage = enemyWeaponDamageBase * 3;
            data.Weapon.IsEquipped = true;
        }

        var enemy = new Character
        {
            UserId = playerCharacter.UserId,
            Name = data.Name,
            Class = data.EnemyClass,
            Level = enemyLevel,
            IsPlayerCharacter = false,
            Strength = (SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.StrengthCoefficient) / enemyCount,
            Stamina = (SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.StaminaCoefficient) / enemyCount,
            Intelligence = (SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.IntelligenceCoefficient) / enemyCount,
            Spirit = (SingleEnemyBaseAttributeAmount + enemyLevel * attributeCoefficients.SpiritCoefficient) / enemyCount,
            BaseArmor = (SingleEnemyBaseMitigationAmount + enemyLevel * attributeCoefficients.ArmorCoefficient) / enemyCount,
            BaseResistance = (SingleEnemyBaseMitigationAmount + enemyLevel * attributeCoefficients.ResistanceCoefficient) / enemyCount,
            Inventory = data.Weapon is not null ? [data.Weapon] : []
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
            return Math.Max(playerCharacterLevel - 1, 1);
        }

        if (RNG.GetBoolean(0.2))
        {
            return playerCharacterLevel + 1;
        }

        return playerCharacterLevel;
    }

    private record EnemyTemplate(
        string Name,
        CharacterClass EnemyClass,
        Weapon? Weapon = null
    );

    private record AttributeCoefficients(
        int StrengthCoefficient,
        int StaminaCoefficient,
        int IntelligenceCoefficient,
        int SpiritCoefficient,
        int ArmorCoefficient,
        int ResistanceCoefficient
    );
}