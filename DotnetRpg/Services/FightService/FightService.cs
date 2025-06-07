using DotnetRpg.Data;
using DotnetRpg.Dtos.Fight;
using DotnetRpg.Dtos.Fights;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Exceptions;
using DotnetRpg.Models.Fights;
using DotnetRpg.Models.Items;
using DotnetRpg.Models.Skills;
using DotnetRpg.Models.StatusEffects;
using DotnetRpg.Services.DamageCalculator;
using DotnetRpg.Services.EnemyGeneratorService;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Services.FightService;

public class FightService : IFightService
{
    private readonly DataContext _context;
    private readonly IEnemyGeneratorService _enemyGeneratorService;
    private readonly IDamageCalculator _damageCalculator;

    public FightService(
        DataContext context, 
        IEnemyGeneratorService enemyGeneratorService, 
        IDamageCalculator damageCalculator)
    {
        _context = context;
        _enemyGeneratorService = enemyGeneratorService;
        _damageCalculator = damageCalculator;
    }

    public async Task<BeginFightResultDto> BeginFight(int playerCharacterId)
    {
        var allCharacters = new List<Character>();
        var playerCharacter = await _context.Characters
                                  .Include(c => c.Fight)
                                  .FirstOrDefaultAsync(c => c.Id == playerCharacterId && c.IsPlayerCharacter) ??
                              throw new NotFoundException("Player character not found");

        if (playerCharacter.Fight != null)
        {
            throw new ConflictException("Player character is already in a fight");
        }

        var enemyCharacters = _enemyGeneratorService.GetEnemies(playerCharacter);

        allCharacters.Add(playerCharacter);
        allCharacters.AddRange(enemyCharacters);

        var newFight = new Fight(playerCharacter.Id, allCharacters);

        await _context.AddAsync(newFight);
        await _context.SaveChangesAsync();

        return new BeginFightResultDto
        {
            FightId = newFight.Id,
            PlayerCharacterId = playerCharacter.Id,
            EnemyCharacterIds = enemyCharacters.Select(c => c.Id).ToList(),
        };
    }

    public async Task<PlayerActionResultDto> UseSkill(PlayerSkillActionDto request)
    {
        var (fight, playerCharacter, targetCharacter, allEnemyCharacters) =
            await GetPlayerActionReferenceData(request);

        var skillInstance =
            playerCharacter.SkillInstances.SingleOrDefault(s => s.Skill.Id == request.SkillId)
            ?? throw new BadRequestException(
                "Invalid action. Player character does not possess this skill"
            );

        ValidatePlayerSkillAction(skillInstance, playerCharacter);

        playerCharacter.CurrentEnergy -= skillInstance.Skill.EnergyCost;

        DamageInstance? damageInstance = null;

        if (skillInstance.Skill.TargetType == SkillTargetType.Enemy) 
        {
            damageInstance = AttackWithSkill(playerCharacter, targetCharacter, skillInstance.Skill);
        }

        var playerActionResult = new PlayerActionResultDto
        {
            PlayerAction = new ActionResultDto
            {
                CharacterId = playerCharacter.Id,
                CharacterName = playerCharacter.Name,
                TargetCharacterId = targetCharacter.Id,
                TargetCharacterName = targetCharacter.Name,
                ActionType = ActionType.Skill,
                SkillName = skillInstance.Skill.Name,
                DamageInstance = damageInstance != null ? DamageInstanceDto.FromDamageInstance(damageInstance) : null,
                Healing = 0
            },
            FightStatus = FightStatus.Ongoing,
        };

        if (targetCharacter.IsDead)
        {
            var allEnemiesDefeated = allEnemyCharacters.All(c => c.IsDead);

            if (allEnemiesDefeated)
            {
                playerActionResult.FightStatus = FightStatus.Victory;
                EndFight(fight, allEnemyCharacters, playerCharacter, playerActionResult, true);
                await _context.SaveChangesAsync();
                return playerActionResult;
            }
        }

        HandleEnemyActions(fight, playerCharacter, allEnemyCharacters, playerActionResult);
        
        // TODO: What happens when a fight is lost?
        
        RegenerateEnergy(fight.AllCharactersInFight);
        UpdateStatusEffectCooldowns(fight.AllCharactersInFight);
        UpdateSkillCooldowns(fight.AllCharactersInFight);

        ApplyStatusEffect(skillInstance.Skill, targetCharacter);
        skillInstance.ApplyCooldown();

        await _context.SaveChangesAsync();

        return playerActionResult;
    }

    public async Task<PlayerActionResultDto> WeaponAttack(PlayerActionDto request)
    {
        var (fight, playerCharacter, targetCharacter, allEnemyCharacters) =
            await GetPlayerActionReferenceData(request);

        var damageInstance = AttackWithWeapon(playerCharacter, targetCharacter);
        var playerActionResult = new PlayerActionResultDto
        {
            PlayerAction = new ActionResultDto
            {
                CharacterId = playerCharacter.Id,
                CharacterName = playerCharacter.Name,
                TargetCharacterId = targetCharacter.Id,
                TargetCharacterName = targetCharacter.Name,
                ActionType = ActionType.WeaponAttack,
                SkillName = null,
                DamageInstance = DamageInstanceDto.FromDamageInstance(damageInstance),
                Healing = 0
            },
            FightStatus = FightStatus.Ongoing,
        };

        if (targetCharacter.IsDead)
        {
            var allEnemiesDefeated = allEnemyCharacters.All(c => c.IsDead);

            if (allEnemiesDefeated)
            {
                EndFight(fight, allEnemyCharacters, playerCharacter, playerActionResult, true);
                await _context.SaveChangesAsync();
                return playerActionResult;
            }
        }

        HandleEnemyActions(fight, playerCharacter, allEnemyCharacters, playerActionResult);
        RegenerateEnergy(fight.AllCharactersInFight);
        UpdateStatusEffectCooldowns(fight.AllCharactersInFight);
        UpdateSkillCooldowns(fight.AllCharactersInFight);

        await _context.SaveChangesAsync();

        return playerActionResult;
    }

    private DamageInstance AttackWithSkill(Character attacker, Character defender, Skill skill)
    {
        if (defender.IsDead)
        {
            throw new BadRequestException(
                $"Invalid action. Target character with ID {defender.Id} has already been defeated"
            );
        }

        var damageInstance = _damageCalculator.CalculateSkillDamage(skill, attacker, defender);

        defender.CurrentHitPoints = Math.Max(defender.CurrentHitPoints - damageInstance.TotalDamage, 0);

        return damageInstance;
    }

    private DamageInstance AttackWithWeapon(Character attacker, Character defender)
    {
        if (defender.IsDead)
        {
            throw new BadRequestException(
                $"Invalid Attack. Target character with ID {defender.Id} has already been defeated"
            );
        }

        var damageInstance = _damageCalculator.CalculateWeaponDamage(attacker, defender);

        defender.CurrentHitPoints = Math.Max(defender.CurrentHitPoints - damageInstance.TotalDamage, 0);

        return damageInstance;
    }

    private void EndFight(
        Fight fight,
        ICollection<Character> allEnemyCharactersInFight,
        Character playerCharacter,
        PlayerActionResultDto playerActionResult,
        bool isVictory
    )
    {
        playerCharacter.CurrentHitPoints = playerCharacter.GetMaxHitPoints();
        playerCharacter.CurrentEnergy = playerCharacter.GetMaxEnergy();
        playerCharacter.SkillInstances.ForEach(s => s.RemainingCooldown = 0);
        playerActionResult.FightStatus = isVictory ? FightStatus.Victory : FightStatus.Defeat;
        
        if (isVictory)
        {
            RewardExperience(playerCharacter, allEnemyCharactersInFight, playerActionResult);
        }
        
        _context.RemoveRange(allEnemyCharactersInFight);
        _context.Remove(fight);
    }

    private void HandleEnemyActions(
        Fight fight,
        Character playerCharacter,
        List<Character> allEnemyCharacters,
        PlayerActionResultDto playerActionResult
    )
    {
        var enemyActions = new List<ActionResultDto>();

        // TODO: Add support for self & friendly targeted skills
        foreach (var enemyCharacter in allEnemyCharacters)
        {
            // Skip action if the character is dead
            if (enemyCharacter.IsDead) continue;
            
            var enemyAction = new ActionResultDto
            {
                CharacterId = enemyCharacter.Id,
                CharacterName = enemyCharacter.Name,
                TargetCharacterId = playerCharacter.Id,
                TargetCharacterName = playerCharacter.Name,
            };
            
            var validSkills = enemyCharacter.SkillInstances
                .Where(
                    s =>
                        s.Skill.TargetType == SkillTargetType.Enemy
                        && s.Skill.EnergyCost <= enemyCharacter.CurrentEnergy
                        && s.RemainingCooldown == 0
                )
                .ToList();

            // 50 / 50 chance to do a skill attack or a weapon attack
            DamageInstance damageInstance;
            if (RNG.GetBoolean(0.5) && validSkills.Any())
            {
                var skillInstance = RNG.PickRandom(validSkills);
                damageInstance = AttackWithSkill(enemyCharacter, playerCharacter, skillInstance.Skill);
                skillInstance.ApplyCooldown();

                enemyAction.ActionType = ActionType.Skill;
                enemyAction.SkillName = skillInstance.Skill.Name;
                enemyAction.DamageInstance = DamageInstanceDto.FromDamageInstance(damageInstance);
                enemyAction.Healing = 0;
            }
            else
            {
                damageInstance = AttackWithWeapon(enemyCharacter, playerCharacter);

                enemyAction.ActionType = ActionType.WeaponAttack;
                enemyAction.SkillName = null;
                enemyAction.DamageInstance = DamageInstanceDto.FromDamageInstance(damageInstance);
                enemyAction.Healing = 0;
            }

            enemyActions.Add(enemyAction);
        }

        playerActionResult.EnemyActions = enemyActions;
        
        if (playerCharacter.IsDead)
        {
            EndFight(fight, allEnemyCharacters, playerCharacter, playerActionResult, false);
        }
    }
    
    private static void RegenerateEnergy(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.CurrentEnergy = Math.Min(character.CurrentEnergy + character.GetEnergyRegeneration(), character.GetMaxEnergy());
        }
    }

    private static void UpdateSkillCooldowns(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.SkillInstances.ForEach(s =>
            {
                if (s.RemainingCooldown > 0) s.RemainingCooldown--;
            });
        }
    }

    private static void UpdateStatusEffectCooldowns(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            // Remove expiring status effects
            character.StatusEffectInstances.RemoveAll(s => s.RemainingDuration <= 1);

            // Decrement remaining durations
            character.StatusEffectInstances.ForEach(s =>
            {
                s.RemainingDuration--;
            });
        }
    }

    private static void ApplyStatusEffect(Skill skill, Character targetCharacter)
    {
        if (skill.StatusEffect is not null)
        {
            targetCharacter.StatusEffectInstances.Add(
                new StatusEffectInstance(targetCharacter.Id, skill.StatusEffect, skill.StatusEffect.Duration)
            );
        }
    }

    private static void RewardExperience(
        Character playerCharacter,
        IEnumerable<Character> enemyCharacters,
        PlayerActionResultDto playerActionResult
    )
    {
        // No more experience gain at max level
        if (playerCharacter.Level == 50) return;

        var averageEnemyLevel = enemyCharacters.Average(s => s.Level);
        var experienceReward = (int)Math.Round(50 * averageEnemyLevel);
        var newExperienceTotal = playerCharacter.Experience + experienceReward;
        playerCharacter.Experience = newExperienceTotal;
        playerActionResult.ExperienceGained = experienceReward;

        var newLevel = LevelExperienceThresholds.AllThresholds
            .Where(t => newExperienceTotal >= t.Value)
            .MaxBy(t => t.Value)
            .Key;

        if (newLevel > playerCharacter.Level)
        {
            playerCharacter.Level = newLevel;
            playerCharacter.UnassignedAttributePoints++;
            playerActionResult.HasLevelUp = true;
        }
    }

    private static void ValidatePlayerSkillAction(SkillInstance skillInstance, Character playerCharacter)
    {
        var playerCharacterIsStunned = playerCharacter.StatusEffectInstances.Any(
                s => s.StatusEffect.IsStunned && s.RemainingDuration > 0
            );

        if (playerCharacterIsStunned)
        {
            throw new BadRequestException("Invalid action. Player character is stunned");
        }

        if (skillInstance.RemainingCooldown > 0)
        {
            throw new BadRequestException("Invalid action. Skill is on cooldown");
        }

        if (playerCharacter.CurrentEnergy < skillInstance.Skill.EnergyCost)
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
                .AsSplitQuery()
                .Include(f => f.AllCharactersInFight)
                .ThenInclude(c => c.SkillInstances)
                .ThenInclude(s => s.Skill)
                .ThenInclude(s => s.StatusEffect)
                .Include(f => f.AllCharactersInFight)
                .ThenInclude(c => c.StatusEffectInstances)
                .ThenInclude(s => s.StatusEffect)
                .Include(f => f.AllCharactersInFight)
                .ThenInclude(c => c.Inventory)
                .SingleOrDefaultAsync(f => f.Id == playerActionDto.FightId)
            ?? throw new BadRequestException("Invalid action. Fight not found");

        var playerCharacter =
            fight.AllCharactersInFight.SingleOrDefault(c => c.IsPlayerCharacter)
            ?? throw new BadRequestException("Invalid action. Player character not found");

        var targetCharacter =
            fight.AllCharactersInFight.Single(c => c.Id == playerActionDto.TargetCharacterId)
            ?? throw new BadRequestException("Invalid action. Target character not found");

        var allEnemyCharactersInFight = fight.AllCharactersInFight.Where(c => !c.IsPlayerCharacter).ToList();

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
