using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.Models.Exceptions;
using dotnet_rpg.Services.EnemyGeneratorService;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService;

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

        var playerCharacter =
            await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.PlayerCharacterId)
            ?? throw new BadRequestException("Invalid action. Player character not found");

        var targetCharacter =
            await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.TargetCharacterId)
            ?? throw new BadRequestException("Invalid action. Target character not found");

        var fight =
            await _context.Fights
                .Include(f => f.Characters)
                .ThenInclude(c => c.Inventory)
                .Include(f => f.Characters)
                .ThenInclude(c => c.Skills)
                .FirstOrDefaultAsync(f => f.Id == request.FightId)
            ?? throw new BadRequestException("Invalid action. Fight not found");

        var skill =
            playerCharacter.Skills.FirstOrDefault(s => s.Id == request.SkillId)
            ?? throw new BadRequestException(
                "Invalid action. Player character does not possess this skill"
            );

        if (skill.RemainingCooldown > 0)
        {
            throw new BadRequestException("Invalid action. Skill is on cooldown");
        }

        if (playerCharacter.CurrentEnergy < skill.EnergyCost)
        {
            throw new BadRequestException("Invalid action. Not enough energy to use this skill");
        }

        playerCharacter.CurrentEnergy -= skill.EnergyCost;

        var damage = 0;

        if (skill.TargetType == SkillTargetType.Enemy)
        {
            if (targetCharacter.CurrentHitPoints <= 0)
            {
                throw new BadRequestException(
                    "Invalid action. Target character has already been defeated"
                );
            }

            damage = AttackWithSkill(playerCharacter, targetCharacter, skill);
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
                SkillName = skill.Name,
                Damage = damage,
                Healing = 0
            },
            FightStatus = FightStatus.Ongoing,
        };

        if (targetCharacter.CurrentHitPoints <= 0)
        {
            var allEnemyCharactersInFight = fight.Characters.Where(c => !c.IsPlayerCharacter);
            var allEnemiesDefeated = allEnemyCharactersInFight.All(c => c.CurrentHitPoints <= 0);

            if (allEnemiesDefeated)
            {
                attackResult.FightStatus = FightStatus.Victory;
                EndFight(fight, allEnemyCharactersInFight, playerCharacter, true);
                await _context.SaveChangesAsync();
                response.Data = attackResult;
                return response;
            }
        }

        HandleEnemyActions(playerCharacter, fight, attackResult);
        RegenerateEnergy(fight.Characters);
        UpdateCooldowns(fight.Characters);
        skill.ApplyCooldown();

        await _context.SaveChangesAsync();

        response.Data = attackResult;

        return response;
    }

    public async Task<ServiceResponse<PlayerActionResultDto>> WeaponAttack(PlayerActionDto request)
    {
        var response = new ServiceResponse<PlayerActionResultDto>();

        var playerCharacter =
            await _context.Characters
                .Include(c => c.Inventory)
                .FirstOrDefaultAsync(c => c.Id == request.PlayerCharacterId)
            ?? throw new BadRequestException("Invalid Attack. Player character not found");

        var enemyCharacter =
            await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.TargetCharacterId)
            ?? throw new BadRequestException("Invalid Attack. Enemy character not found");

        var fight =
            await _context.Fights
                .Include(f => f.Characters)
                .ThenInclude(c => c.Inventory)
                .Include(f => f.Characters)
                .ThenInclude(c => c.Skills)
                .FirstOrDefaultAsync(f => f.Id == request.FightId)
            ?? throw new BadRequestException("Invalid Attack. Fight not found");

        if (enemyCharacter.CurrentHitPoints <= 0)
        {
            throw new BadRequestException(
                "Invalid Attack. Enemy character has already been defeated"
            );
        }

        var damage = AttackWithWeapon(playerCharacter, enemyCharacter);
        var attackResult = new PlayerActionResultDto
        {
            PlayerAction = new ActionResultDto
            {
                CharacterId = playerCharacter.Id,
                CharacterName = playerCharacter.Name,
                TargetCharacterId = enemyCharacter.Id,
                TargetCharacterName = enemyCharacter.Name,
                ActionType = ActionType.WeaponAttack,
                SkillName = null,
                Damage = damage,
                Healing = 0
            },
            FightStatus = FightStatus.Ongoing,
        };

        if (enemyCharacter.CurrentHitPoints <= 0)
        {
            var allEnemyCharactersInFight = fight.Characters.Where(c => !c.IsPlayerCharacter);
            var allEnemiesDefeated = allEnemyCharactersInFight.All(c => c.CurrentHitPoints <= 0);

            if (allEnemiesDefeated)
            {
                attackResult.FightStatus = FightStatus.Victory;
                EndFight(fight, allEnemyCharactersInFight, playerCharacter, true);
                response.Data = attackResult;
                await _context.SaveChangesAsync();
                return response;
            }
        }

        HandleEnemyActions(playerCharacter, fight, attackResult);
        RegenerateEnergy(fight.Characters);
        UpdateCooldowns(fight.Characters);

        await _context.SaveChangesAsync();

        response.Data = attackResult;

        return response;
    }

    private static int AttackWithSkill(Character attacker, Character defender, Skill skill)
    {
        if (skill.StatusEffect != null)
        {
            defender.StatusEffects.Add(skill.StatusEffect);
        }

        var damage = CalculateSkillDamage(skill, attacker, defender);

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
        playerCharacter.Skills.ForEach(s => s.RemainingCooldown = 0);

        if (isVictory)
        {
            RewardExperience(playerCharacter, allEnemyCharactersInFight);
        }

        // TODO: What happens when a fight is lost?
    }

    private void HandleEnemyActions(
        Character playerCharacter,
        Fight fight,
        PlayerActionResultDto resultDto
    )
    {
        var allEnemyCharactersInFight = fight.Characters.Where(c => !c.IsPlayerCharacter);
        var damage = 0;
        var enemyActions = new List<ActionResultDto>();

        // TODO: Add support for self & friendly targeted skills
        foreach (var enemyCharacter in allEnemyCharactersInFight)
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
                var validSkills = enemyCharacter.Skills
                    .Where(
                        s =>
                            s.TargetType == SkillTargetType.Enemy
                            && s.EnergyCost <= enemyCharacter.CurrentEnergy
                            && s.RemainingCooldown == 0
                    )
                    .ToList();

                // 50 / 50 change to do a skill attack or a weapon attack
                if (RNG.GetBoolean(0.5) && validSkills.Any())
                {
                    var skill = RNG.PickRandom(validSkills);
                    damage = AttackWithSkill(enemyCharacter, playerCharacter, skill);
                    skill.ApplyCooldown();

                    enemyAction.ActionType = ActionType.Skill;
                    enemyAction.SkillName = skill.Name;
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
            EndFight(fight, allEnemyCharactersInFight, playerCharacter, false);
        }

        resultDto.EnemyActions = enemyActions;
    }

    private static int CalculateSkillDamage(Skill skill, Character attacker, Character defender)
    {
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

        var attackerWeapon = attacker.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);
        var skillDamage = skill.GetSkillDamage(attackerWeapon, attacker);
        var damageMultiplier = 1 - damageReduction;
        var damage = (int)Math.Round(skillDamage * damageMultiplier);

        return damage;
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

    private static void UpdateCooldowns(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.Skills.ForEach(s =>
            {
                if (s.RemainingCooldown > 0)
                {
                    s.RemainingCooldown--;
                }
            });
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
}
