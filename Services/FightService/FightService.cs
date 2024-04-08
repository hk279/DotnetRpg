using DotnetRpg.Data;
using DotnetRpg.Dtos.Fight;
using DotnetRpg.Models.Exceptions;
using DotnetRpg.Services.EnemyGeneratorService;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Services.FightService;

public class FightService : IFightService
{
    private readonly DataContext _context;
    private readonly IEnemyGeneratorService _enemyGeneratorService;

    public FightService(DataContext context, IEnemyGeneratorService enemyGeneratorService)
    {
        _context = context;
        _enemyGeneratorService = enemyGeneratorService;
    }

    public async Task<ServiceResponse<BeginFightResultDto>> BeginFight(int characterId)
    {
        var response = new ServiceResponse<BeginFightResultDto>();

        var allCharacters = new List<Character>();
        var playerCharacter =
            await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId)
            ?? throw new NotFoundException("Player character not found");

        if (playerCharacter.FightId != null)
        {
            throw new ConflictException("Player character is already in a fight");
        }

        var enemyCharacters = _enemyGeneratorService.GetEnemies(playerCharacter.Level);

        allCharacters.Add(playerCharacter);
        allCharacters.AddRange(enemyCharacters);

        await _context.SaveChangesAsync();

        var newFight = new Fight() { Characters = allCharacters };

        await _context.AddAsync(newFight);
        await _context.SaveChangesAsync();

        response.Data = new BeginFightResultDto
        {
            Id = newFight.Id,
            PlayerCharacterId = playerCharacter.Id,
            EnemyCharacterIds = enemyCharacters.Select(c => c.Id).ToList(),
        };

        return response;
    }

    public async Task<ServiceResponse<PlayerActionResultDto>> UseSkill(PlayerSkillActionDto request)
    {
        var response = new ServiceResponse<PlayerActionResultDto>();

        var (fight, playerCharacter, targetCharacter, allEnemyCharacters) =
            await GetPlayerActionReferenceData(request);

        var skillInstance =
            playerCharacter.SkillInstances.SingleOrDefault(s => s.Skill.Id == request.SkillId)
            ?? throw new BadRequestException(
                "Invalid action. Player character does not possess this skill"
            );

        ValidateSkillAction(skillInstance, playerCharacter);

        playerCharacter.CurrentEnergy -= skillInstance.Skill.EnergyCost;

        var damage = 0;

        if (skillInstance.Skill.TargetType == SkillTargetType.Enemy)
        {
            damage = AttackWithSkill(playerCharacter, targetCharacter, skillInstance.Skill);
        }

        var attackResult = new PlayerActionResultDto
        {
            PlayerAction = new ActionResultDto
            {
                CharacterId = playerCharacter.Id,
                CharacterName = playerCharacter.Name,
                TargetCharacterId = targetCharacter.Id,
                TargetCharacterName = targetCharacter.Name,
                ActionType = ActionType.Skill,
                SkillName = skillInstance.Skill.Name,
                Damage = damage,
                Healing = 0
            },
            FightStatus = FightStatus.Ongoing,
        };

        if (targetCharacter.CurrentHitPoints <= 0)
        {
            var allEnemiesDefeated = allEnemyCharacters.All(c => c.CurrentHitPoints <= 0);

            if (allEnemiesDefeated)
            {
                attackResult.FightStatus = FightStatus.Victory;
                EndFight(fight, allEnemyCharacters, playerCharacter, true);
                await _context.SaveChangesAsync();
                response.Data = attackResult;
                return response;
            }
        }

        HandleEnemyActions(fight, playerCharacter, allEnemyCharacters, attackResult);
        RegenerateEnergy(fight.Characters);
        UpdateStatusEffectCooldowns(fight.Characters);
        UpdateSkillCooldowns(fight.Characters);

        ApplyStatusEffect(skillInstance.Skill, targetCharacter);
        skillInstance.ApplyCooldown();

        await _context.SaveChangesAsync();

        response.Data = attackResult;

        return response;
    }

    public async Task<ServiceResponse<PlayerActionResultDto>> WeaponAttack(PlayerActionDto request)
    {
        var response = new ServiceResponse<PlayerActionResultDto>();

        var (fight, playerCharacter, targetCharacter, allEnemyCharacters) =
            await GetPlayerActionReferenceData(request);

        var damage = AttackWithWeapon(playerCharacter, targetCharacter);
        var attackResult = new PlayerActionResultDto
        {
            PlayerAction = new ActionResultDto
            {
                CharacterId = playerCharacter.Id,
                CharacterName = playerCharacter.Name,
                TargetCharacterId = targetCharacter.Id,
                TargetCharacterName = targetCharacter.Name,
                ActionType = ActionType.WeaponAttack,
                SkillName = null,
                Damage = damage,
                Healing = 0
            },
            FightStatus = FightStatus.Ongoing,
        };

        if (targetCharacter.CurrentHitPoints <= 0)
        {
            var allEnemiesDefeated = allEnemyCharacters.All(c => c.CurrentHitPoints <= 0);

            if (allEnemiesDefeated)
            {
                attackResult.FightStatus = FightStatus.Victory;
                EndFight(fight, allEnemyCharacters, playerCharacter, true);
                response.Data = attackResult;
                await _context.SaveChangesAsync();
                return response;
            }
        }

        HandleEnemyActions(fight, playerCharacter, allEnemyCharacters, attackResult);
        RegenerateEnergy(fight.Characters);
        UpdateStatusEffectCooldowns(fight.Characters);
        UpdateSkillCooldowns(fight.Characters);

        await _context.SaveChangesAsync();

        response.Data = attackResult;

        return response;
    }

    private static int AttackWithSkill(Character attacker, Character defender, Skill skill)
    {
        if (defender.CurrentHitPoints <= 0)
        {
            throw new BadRequestException(
                $"Invalid action. Target character with ID {defender.Id} has already been defeated"
            );
        }

        var damage = CalculateSkillDamage(skill, attacker, defender);

        // Apply damage
        if (damage > 0)
        {
            defender.CurrentHitPoints -= damage;

            if (defender.CurrentHitPoints < 0)
            {
                defender.CurrentHitPoints = 0;
            }
        }

        return damage;
    }

    private static int AttackWithWeapon(Character attacker, Character defender)
    {
        if (defender.CurrentHitPoints <= 0)
        {
            throw new BadRequestException(
                "Invalid Attack. Target character has already been defeated"
            );
        }

        var damage = CalculateWeaponDamage(attacker, defender);

        if (damage > 0)
        {
            defender.CurrentHitPoints -= damage;

            if (defender.CurrentHitPoints < 0)
            {
                defender.CurrentHitPoints = 0;
            }
        }

        return damage;
    }

    private void EndFight(
        Fight fight,
        IEnumerable<Character> allEnemyCharactersInFight,
        Character playerCharacter,
        bool isVictory
    )
    {
        _context.RemoveRange(allEnemyCharactersInFight);
        _context.Remove(fight);

        playerCharacter.CurrentHitPoints = playerCharacter.GetMaxHitPoints();
        playerCharacter.CurrentEnergy = playerCharacter.GetMaxEnergy();
        playerCharacter.SkillInstances.ForEach(s => s.RemainingCooldown = 0);

        if (isVictory)
        {
            RewardExperience(playerCharacter, allEnemyCharactersInFight);
        }

        // TODO: What happens when a fight is lost?
    }

    private void HandleEnemyActions(
        Fight fight,
        Character playerCharacter,
        List<Character> allEnemyCharacters,
        PlayerActionResultDto resultDto
    )
    {
        var damage = 0;
        var enemyActions = new List<ActionResultDto>();

        // TODO: Add support for self & friendly targeted skills
        foreach (var enemyCharacter in allEnemyCharacters)
        {
            var enemyAction = new ActionResultDto
            {
                CharacterId = enemyCharacter.Id,
                CharacterName = enemyCharacter.Name,
                TargetCharacterId = playerCharacter.Id,
                TargetCharacterName = playerCharacter.Name,
            };

            if (enemyCharacter.CurrentHitPoints > 0)
            {
                var validSkills = enemyCharacter.SkillInstances
                    .Where(
                        s =>
                            s.Skill.TargetType == SkillTargetType.Enemy
                            && s.Skill.EnergyCost <= enemyCharacter.CurrentEnergy
                            && s.RemainingCooldown == 0
                    )
                    .ToList();

                // 50 / 50 change to do a skill attack or a weapon attack
                if (RNG.GetBoolean(0.5) && validSkills.Any())
                {
                    var skillInstance = RNG.PickRandom(validSkills);
                    damage = AttackWithSkill(enemyCharacter, playerCharacter, skillInstance.Skill);
                    skillInstance.ApplyCooldown();

                    enemyAction.ActionType = ActionType.Skill;
                    enemyAction.SkillName = skillInstance.Skill.Name;
                    enemyAction.Damage = damage;
                    enemyAction.Healing = 0;
                }
                else
                {
                    damage = AttackWithWeapon(enemyCharacter, playerCharacter);

                    enemyAction.ActionType = ActionType.WeaponAttack;
                    enemyAction.SkillName = null;
                    enemyAction.Damage = damage;
                    enemyAction.Healing = 0;
                }
            }

            enemyActions.Add(enemyAction);
        }

        if (playerCharacter.CurrentHitPoints <= 0)
        {
            resultDto.FightStatus = FightStatus.Defeat;
            EndFight(fight, allEnemyCharacters, playerCharacter, false);
        }

        resultDto.EnemyActions = enemyActions;
    }

    /// <summary>
    /// A skill has weapon and base damage components. Both components are scaled by character attributes independently.
    /// Damage is scaled down by the defender's armor / resistance.
    /// </summary>
    private static int CalculateSkillDamage(Skill skill, Character attacker, Character defender)
    {
        var scalingAttributeAmount = skill.DamageType switch
        {
            DamageType.Physical => attacker.GetStrength(),
            DamageType.Magic => attacker.GetIntelligence(),
            _ => throw new ArgumentOutOfRangeException(nameof(DamageType), "Unknown damage type")
        };

        // Calculate weapon damage component
        var attackerWeapon = attacker.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);
        var attackerWeaponDamage =
            attackerWeapon != null
                ? RNG.GetIntInRange(attackerWeapon.MinDamage, attackerWeapon.MaxDamage)
                : 1;
        var skillWeaponDamageComponent =
            attackerWeaponDamage * (skill.WeaponDamagePercentage / 100)
            + (scalingAttributeAmount * (skill.BaseDamageAttributeScalingFactor / 100) / 2);

        // Calculate skill base damage component
        var levelAdjustedMinimumBaseDamage = attacker.Level * (skill.MinBaseDamageFactor / 10);
        var levelAdjustedMaximumBaseDamage = attacker.Level * (skill.MaxBaseDamageFactor / 10);

        var levelAdjustedBaseDamage = RNG.GetIntInRange(
            levelAdjustedMinimumBaseDamage,
            levelAdjustedMaximumBaseDamage
        );

        var skillBaseDamageComponent =
            levelAdjustedBaseDamage
            + (scalingAttributeAmount * (skill.BaseDamageAttributeScalingFactor / 100) / 2);

        var skillDamage = skillWeaponDamageComponent + skillBaseDamageComponent;

        // Calculate damage reduction
        var damageReduction = skill.DamageType switch
        {
            DamageType.Physical => Math.Round((decimal)defender.GetArmor() / 100, 2),
            DamageType.Magic => Math.Round((decimal)defender.GetResistance() / 100, 2),
            _
                => throw new ArgumentOutOfRangeException(
                    nameof(skill.DamageType),
                    "Invalid damage type"
                )
        };
        var damageReductionFactor = 1 - damageReduction;

        var totalDamage = (int)Math.Round(skillDamage * damageReductionFactor);

        return totalDamage;
    }

    private static int CalculateWeaponDamage(Character attacker, Character defender)
    {
        // Weapons always deal physical damage
        var damageBonus = Math.Round((decimal)attacker.GetStrength() / 100, 2);
        var damageReduction = Math.Round((decimal)defender.GetArmor() / 100, 2);
        var attackerWeapon = attacker.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);

        var baseDamage = RNG.GetIntInRange(
            attackerWeapon?.MinDamage ?? 1,
            attackerWeapon?.MaxDamage ?? 1
        );
        var damageMultiplier = 1 + damageBonus - damageReduction;
        var damage = (int)Math.Round(baseDamage * damageMultiplier);

        return damage;
    }

    private static void RegenerateEnergy(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.CurrentEnergy += character.GetSpirit();

            if (character.CurrentEnergy > character.GetMaxEnergy())
            {
                character.CurrentEnergy = character.GetMaxEnergy();
            }
        }
    }

    private static void UpdateSkillCooldowns(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.SkillInstances.ForEach(s =>
            {
                if (s.RemainingCooldown > 0)
                {
                    s.RemainingCooldown--;
                }
            });
        }
    }

    private static void UpdateStatusEffectCooldowns(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            // Remove expiring status effects
            character.StatusEffectInstances.RemoveAll(s => s.RemainingDuration == 1);

            // Decrement remaining durations
            character.StatusEffectInstances.ForEach(s =>
            {
                s.RemainingDuration--;
            });
        }
    }

    private static void ApplyStatusEffect(Skill skill, Character targetCharacter)
    {
        if (skill.StatusEffect != null)
        {
            targetCharacter.StatusEffectInstances.Add(
                new StatusEffectInstance(skill.StatusEffect, skill.StatusEffect.Duration)
            );
        }
    }

    private static void RewardExperience(
        Character playerCharacter,
        IEnumerable<Character> enemyCharacters
    )
    {
        if (playerCharacter.Level == 50)
        {
            return;
        }

        var averageEnemyLevel = enemyCharacters.Average(s => s.Level);
        var experienceReward = (long)Math.Round(10 * averageEnemyLevel);
        var newExperienceTotal = playerCharacter.Experience + experienceReward;
        var newLevel = LevelExperienceThresholds.AllThresholds
            .Where(t => newExperienceTotal >= t.Value)
            .MaxBy(t => t.Value)
            .Key;

        playerCharacter.Experience = newExperienceTotal;
        playerCharacter.Level = newLevel;
    }

    private static void ValidateSkillAction(SkillInstance skillInstance, Character character)
    {
        if (
            character.StatusEffectInstances.Any(
                s => s.StatusEffect.IsStunned && s.RemainingDuration > 0
            )
        )
        {
            throw new BadRequestException("Invalid action. Character is stunned");
        }

        if (skillInstance.RemainingCooldown > 0)
        {
            throw new BadRequestException("Invalid action. Skill is on cooldown");
        }

        if (character.CurrentEnergy < skillInstance.Skill.EnergyCost)
        {
            throw new BadRequestException("Invalid action. Not enough energy to use this skill");
        }
    }

    private async Task<PlayerActionReferenceData> GetPlayerActionReferenceData(
        PlayerActionDto playerActionDto
    )
    {
        var fight =
            await _context.Fights
                .Include(f => f.Characters)
                .ThenInclude(c => c.SkillInstances)
                .ThenInclude(s => s.Skill)
                .ThenInclude(s => s.StatusEffect)
                .Include(f => f.Characters)
                .ThenInclude(c => c.StatusEffectInstances)
                .ThenInclude(s => s.StatusEffect)
                .Include(f => f.Characters)
                .ThenInclude(c => c.Inventory)
                .SingleOrDefaultAsync(f => f.Id == playerActionDto.FightId)
            ?? throw new BadRequestException("Invalid action. Fight not found");

        var playerCharacter =
            fight.Characters.SingleOrDefault(c => c.IsPlayerCharacter)
            ?? throw new BadRequestException("Invalid action. Player character not found");

        var targetCharacter =
            fight.Characters.Single(c => c.Id == playerActionDto.TargetCharacterId)
            ?? throw new BadRequestException("Invalid action. Target character not found");

        var allEnemyCharactersInFight = fight.Characters.Where(c => !c.IsPlayerCharacter).ToList();

        return new PlayerActionReferenceData(
            fight,
            playerCharacter,
            targetCharacter,
            allEnemyCharactersInFight
        );
    }
}

public record PlayerActionReferenceData(
    Fight Fight,
    Character PlayerCharacter,
    Character TargetCharacter,
    List<Character> AllEnemyCharacters
);
