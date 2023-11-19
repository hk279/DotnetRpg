namespace dotnet_rpg.Data.Seeding;

public class SkillDataSeeder
{
    public static (List<Skill>, List<StatusEffect>) GetWarriorSkills()
    {
        var warriorSkills = new List<Skill>();
        var warriorSkillStatusEffects = new List<StatusEffect>();

        // TODO: Add other skills
        var chargeSkills = GetChargeSkills();
        warriorSkills.AddRange(chargeSkills);

        // Apply skill IDs
        warriorSkills = warriorSkills
            .Select(
                (s, index) =>
                {
                    s.Id = index + 1;
                    return s;
                }
            )
            .ToList();

        // TODO: Add status effects for other skills
        var chargeSkillStatusEffects = chargeSkills.Select(
            s => new StatusEffect("Charge Stun", duration: 1, skillId: s.Id, isStunned: true)
        );
        warriorSkillStatusEffects.AddRange(chargeSkillStatusEffects);

        // Apply status effect IDs
        warriorSkillStatusEffects = warriorSkillStatusEffects
            .Select(
                (s, index) =>
                {
                    s.Id = index + 1;
                    return s;
                }
            )
            .ToList();

        return (warriorSkills, warriorSkillStatusEffects);
    }

    private static List<Skill> GetChargeSkills()
    {
        return new List<Skill>
        {
            CreateChargeSkill(
                rank: 1,
                weaponDamagePercentage: 10,
                minBaseDamageFactor: 30,
                maxBaseDamageFactor: 40,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 5
            ),
            CreateChargeSkill(
                rank: 2,
                weaponDamagePercentage: 20,
                minBaseDamageFactor: 40,
                maxBaseDamageFactor: 50,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 5
            ),
            CreateChargeSkill(
                rank: 3,
                weaponDamagePercentage: 30,
                minBaseDamageFactor: 40,
                maxBaseDamageFactor: 50,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 5
            ),
        };
    }

    private static Skill CreateChargeSkill(
        int rank,
        int weaponDamagePercentage,
        int minBaseDamageFactor,
        int maxBaseDamageFactor,
        int baseDamageAttributeScalingFactor,
        int energyCost,
        int cooldown
    )
    {
        return new Skill(
            CharacterClass.Warrior,
            "Charge",
            "Violently charge the enemy.",
            DamageType.Physical,
            SkillTargetType.Enemy,
            rank,
            weaponDamagePercentage,
            minBaseDamageFactor,
            maxBaseDamageFactor,
            baseDamageAttributeScalingFactor,
            energyCost,
            cooldown
        );
    }
}
