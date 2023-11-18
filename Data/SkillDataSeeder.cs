namespace dotnet_rpg.Data;

public class SkillDataSeeder
{
    private readonly DataContext _dbContext;

    public SkillDataSeeder(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    // TODO: Add seeding on startup
    public async Task SeedSkills()
    {
        await _dbContext.Skills.AddRangeAsync(GetWarriorSkills());
        await _dbContext.SaveChangesAsync();
    }

    private static IEnumerable<Skill> GetWarriorSkills()
    {
        // Earlier skills should stay relevant through the levels.
        // => All skill effects are scaled based on level and new ranks simply improve the scaling

        // TODO: Make more concise
        var noviceSkills = new List<Skill>
        {
            new(
                CharacterClass.Warrior,
                "Charge",
                1,
                DamageType.Physical,
                SkillTargetType.Enemy,
                weaponDamagePercentage: 10,
                minBaseDamageFactor: 30,
                maxBaseDamageFactor: 40,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 5,
                statusEffect: new StatusEffect("Charge Stun", 1, stunned: 1)
            ),
            new(
                CharacterClass.Warrior,
                "Charge",
                2,
                DamageType.Physical,
                SkillTargetType.Enemy,
                weaponDamagePercentage: 20,
                minBaseDamageFactor: 40,
                maxBaseDamageFactor: 50,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 5,
                statusEffect: new StatusEffect("Charge Stun", 1, stunned: 1)
            ),
            new(
                CharacterClass.Warrior,
                "Charge",
                3,
                DamageType.Physical,
                SkillTargetType.Enemy,
                weaponDamagePercentage: 30,
                minBaseDamageFactor: 40,
                maxBaseDamageFactor: 50,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 5,
                statusEffect: new StatusEffect("Charge Stun", 1, stunned: 1)
            ),
        };

        // TODO: Add other skills

        return new List<Skill>();
    }
}
