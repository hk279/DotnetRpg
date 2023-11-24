namespace dotnet_rpg.Data.Seeding;

public class SkillDataSeeder
{
    /// <summary>
    /// Two-step seeding:
    /// 1. Generate skills and give them IDs
    /// 2. Generate status effects for skills and connect them to skills using the skill ID
    /// </summary>
    public static (List<Skill>, List<StatusEffect>) GetWarriorSkills()
    {
        var warriorSkills = new List<Skill>();
        var warriorSkillStatusEffects = new List<StatusEffect>();

        // TODO: Add other skills
        var chargeSkills = GetChargeSkills();
        var rendSkills = GetRendSkills();

        warriorSkills.AddRange(chargeSkills);
        warriorSkills.AddRange(rendSkills);

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
        warriorSkillStatusEffects.AddRange(GetChargeSkillStatusEffects(chargeSkills));
        warriorSkillStatusEffects.AddRange(GetRendSkillStatusEffects(rendSkills));

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

    private static List<StatusEffect> GetChargeSkillStatusEffects(List<Skill> chargeSkills)
    {
        return chargeSkills
            .Select(
                s =>
                    new StatusEffect(
                        "Stunned",
                        duration: 1,
                        StatusEffectType.Physical,
                        skillId: s.Id,
                        isStunned: true
                    )
            )
            .ToList();
    }

    private static List<Skill> GetRendSkills()
    {
        return new List<Skill>()
        {
            CreateRendSkill(1, 30, 40, 50, 50, 20, 4),
            CreateRendSkill(2, 50, 40, 50, 50, 20, 4),
            CreateRendSkill(3, 50, 40, 50, 50, 20, 4),
        };
    }

    private static List<StatusEffect> GetRendSkillStatusEffects(List<Skill> rendSkills)
    {
        return rendSkills
            .Select(s =>
            {
                return s.Rank switch
                {
                    1
                        => new StatusEffect(
                            "Bleeding",
                            duration: 3,
                            StatusEffectType.Physical,
                            s.Id,
                            damagePerTurnFactor: 30
                        ),
                    2
                        => new StatusEffect(
                            "Bleeding",
                            duration: 3,
                            StatusEffectType.Physical,
                            s.Id,
                            damagePerTurnFactor: 30
                        ),
                    3
                        => new StatusEffect(
                            "Bleeding",
                            duration: 4,
                            StatusEffectType.Physical,
                            s.Id,
                            damagePerTurnFactor: 30
                        ),
                    _
                        => throw new ArgumentOutOfRangeException(
                            nameof(s.Rank),
                            $"Unexpected skill rank value: {s.Rank}"
                        ),
                };
            })
            .ToList();
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
            energyCost,
            cooldown,
            weaponDamagePercentage,
            minBaseDamageFactor,
            maxBaseDamageFactor,
            baseDamageAttributeScalingFactor
        );
    }

    private static Skill CreateRendSkill(
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
            "Rend",
            "Slash at your opponent, causing a grievous wound.",
            DamageType.Physical,
            SkillTargetType.Enemy,
            rank,
            energyCost,
            cooldown,
            weaponDamagePercentage,
            minBaseDamageFactor,
            maxBaseDamageFactor,
            baseDamageAttributeScalingFactor
        );
    }
}
