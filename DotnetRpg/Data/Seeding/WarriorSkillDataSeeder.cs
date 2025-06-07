using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Skills;
using DotnetRpg.Models.StatusEffects;

namespace DotnetRpg.Data.Seeding;

public static class WarriorSkillDataSeeder
{
    public static List<Skill> GetWarriorSkills()
    {
        var warriorSkills = new List<Skill>();

        warriorSkills.AddRange(GetChargeSkills());
        warriorSkills.AddRange(GetRendSkills());

        return warriorSkills;
    }

    private static List<Skill> GetChargeSkills()
    {
        return
        [
            CreateChargeSkill(
                rank: 1,
                weaponDamagePercentage: 10,
                minBaseDamageFactor: 80,
                maxBaseDamageFactor: 100,
                baseDamageAttributeScalingFactor: 80,
                energyCost: 15,
                cooldown: 6,
                new StatusEffect("Stunned", duration: 1, StatusEffectType.Physical, isStunned: true)
            ),
            CreateChargeSkill(
                rank: 2,
                weaponDamagePercentage: 20,
                minBaseDamageFactor: 120,
                maxBaseDamageFactor: 160,
                baseDamageAttributeScalingFactor: 90,
                energyCost: 15,
                cooldown: 6,
                new StatusEffect("Stunned", duration: 1, StatusEffectType.Physical, isStunned: true)
            ),
            CreateChargeSkill(
                rank: 3,
                weaponDamagePercentage: 30,
                minBaseDamageFactor: 150,
                maxBaseDamageFactor: 200,
                baseDamageAttributeScalingFactor: 100,
                energyCost: 15,
                cooldown: 6,
                new StatusEffect("Stunned", duration: 1, StatusEffectType.Physical, isStunned: true)
            )
        ];
    }

    private static List<Skill> GetRendSkills()
    {
        return
        [
            CreateRendSkill(1, 30, 40, 50, 50, 20, 4, new StatusEffect(
                "Bleeding",
                duration: 3,
                StatusEffectType.Physical
            // damagePerTurnFactor: 30
            )),
            CreateRendSkill(2, 50, 60, 80, 60, 20, 4, new StatusEffect(
                "Bleeding",
                duration: 3,
                StatusEffectType.Physical
            // damagePerTurnFactor: 30
            )),
            CreateRendSkill(3, 70, 80, 100, 70, 20, 4, new StatusEffect(
                "Bleeding",
                duration: 4,
                StatusEffectType.Physical
            // damagePerTurnFactor: 30
            ))
        ];
    }

    private static Skill CreateChargeSkill(
        int rank,
        int weaponDamagePercentage,
        int minBaseDamageFactor,
        int maxBaseDamageFactor,
        int baseDamageAttributeScalingFactor,
        int energyCost,
        int cooldown,
        StatusEffect? statusEffect = null
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
            baseDamageAttributeScalingFactor,
            statusEffect
        );
    }

    private static Skill CreateRendSkill(
        int rank,
        int weaponDamagePercentage,
        int minBaseDamageFactor,
        int maxBaseDamageFactor,
        int baseDamageAttributeScalingFactor,
        int energyCost,
        int cooldown,
        StatusEffect? statusEffect = null
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
            baseDamageAttributeScalingFactor,
            statusEffect
        );
    }
}
