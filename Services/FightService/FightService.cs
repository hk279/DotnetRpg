using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Fight;
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

        try
        {
            var allCharacters = new List<Character>();
            var playerCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId)
            ?? throw new Exception("Player character not found.");

            if (playerCharacter.FightId != null) throw new Exception("Player is already in a fight.");

            var enemyCharacters = _enemyGeneratorService.GetEnemies(playerCharacter.Level);

            allCharacters.Add(playerCharacter);
            allCharacters.AddRange(enemyCharacters);

            await _context.SaveChangesAsync();

            // TODO: Add a reward for winning the fight
            var newFight = new Fight()
            {
                Characters = allCharacters
            };

            await _context.AddAsync(newFight);
            await _context.SaveChangesAsync();

            response.Data = new BeginFightResultDto
            {
                Id = newFight.Id,
                PlayerCharacterId = playerCharacter.Id,
                EnemyCharacterIds = enemyCharacters.Select(c => c.Id).ToList(),
            };
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<PlayerActionResultDto>> SkillAttack(SkillAttackDto request)
    {
        var response = new ServiceResponse<PlayerActionResultDto>();

        try
        {
            var playerCharacter = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.PlayerCharacterId);
            var enemyCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.EnemyCharacterId);
            var fight = await _context.Fights
                .Include(f => f.Characters)
                    .ThenInclude(c => c.Weapon)
                .Include(f => f.Characters)
                    .ThenInclude(c => c.Skills)
                .FirstOrDefaultAsync(f => f.Id == request.FightId);

            if (playerCharacter == null || enemyCharacter == null || fight == null) throw new Exception("Invalid attack");

            var skill = playerCharacter.Skills.FirstOrDefault(s => s.Id == request.SkillId)
                ?? throw new Exception("Invalid skill. Attacker doesn't possess this skill.");

            if (skill.TargetType != SkillTargetType.Enemy) throw new Exception("Skill cannot be used on an enemy");
            if (skill.RemainingCooldown > 0) throw new Exception("Skill is on cooldown");

            var damage = AttackWithSkill(playerCharacter, enemyCharacter, skill);
            var attackResult = new PlayerActionResultDto
            {
                PlayerAction = new ActionDto
                {
                    CharacterId = playerCharacter.Id,
                    CharacterName = playerCharacter.Name,
                    TargetCharacterId = enemyCharacter.Id,
                    TargetCharacterName = enemyCharacter.Name,
                    ActionType = ActionType.Skill,
                    SkillName = skill.Name,
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
                    EndFight(fight, allEnemyCharactersInFight, playerCharacter);
                    response.Data = attackResult;
                    return response;
                };
            }

            HandleEnemyActions(playerCharacter, fight, attackResult);
            RegenerateEnergy(fight.Characters);
            UpdateCooldowns(fight.Characters);
            skill.ApplyCooldown();

            await _context.SaveChangesAsync();

            response.Data = attackResult;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<PlayerActionResultDto>> WeaponAttack(AttackDto request)
    {
        var response = new ServiceResponse<PlayerActionResultDto>();

        try
        {
            var playerCharacter = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.PlayerCharacterId);
            var enemyCharacter = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.EnemyCharacterId);
            var fight = await _context.Fights
                .Include(f => f.Characters)
                    .ThenInclude(c => c.Weapon)
                .Include(f => f.Characters)
                    .ThenInclude(c => c.Skills)
                .FirstOrDefaultAsync(f => f.Id == request.FightId);

            if (playerCharacter == null || enemyCharacter == null || fight == null) throw new Exception("Invalid attack");
            if (enemyCharacter.CurrentHitPoints <= 0) throw new Exception("Defender is already defeated");

            var damage = AttackWithWeapon(playerCharacter, enemyCharacter);
            var attackResult = new PlayerActionResultDto
            {
                PlayerAction = new ActionDto
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
                    EndFight(fight, allEnemyCharactersInFight, playerCharacter);
                    response.Data = attackResult;
                    return response;
                };
            }

            HandleEnemyActions(playerCharacter, fight, attackResult);
            RegenerateEnergy(fight.Characters);
            UpdateCooldowns(fight.Characters);

            await _context.SaveChangesAsync();

            response.Data = attackResult;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    private static int AttackWithSkill(Character attacker, Character defender, Skill skill)
    {
        if (attacker.CurrentEnergy < skill.EnergyCost) throw new Exception("Not enough energy");

        attacker.CurrentEnergy -= skill.EnergyCost;

        var damage = CalculateSkillDamage(skill, attacker, defender);

        if (damage > 0)
        {
            defender.CurrentHitPoints -= damage;
            if (defender.CurrentHitPoints < 0) defender.CurrentHitPoints = 0;
        }

        return damage;
    }

    private static int AttackWithWeapon(Character attacker, Character defender)
    {
        var damage = CalculateWeaponDamage(attacker, defender);

        if (damage > 0)
        {
            defender.CurrentHitPoints -= damage;
            if (defender.CurrentHitPoints < 0) defender.CurrentHitPoints = 0;
        }

        return damage;
    }

    private void EndFight(Fight fight, IEnumerable<Character> allEnemyCharactersInFight, Character playerCharacter)
    {
        _context.RemoveRange(allEnemyCharactersInFight);
        _context.Remove(fight);

        playerCharacter.CurrentHitPoints = playerCharacter.MaxHitPoints;
        playerCharacter.CurrentEnergy = playerCharacter.MaxEnergy;
        playerCharacter.Skills.ForEach(s => s.RemainingCooldown = 0);
    }

    private void HandleEnemyActions(Character playerCharacter, Fight fight, PlayerActionResultDto resultDto)
    {
        var allEnemyCharactersInFight = fight.Characters.Where(c => !c.IsPlayerCharacter);
        var damage = 0;
        var enemyActions = new List<ActionDto>();

        // TODO: Add support for self & friendly targeted skills
        foreach (var enemyCharacter in allEnemyCharactersInFight)
        {
            var enemyAction = new ActionDto
            {
                CharacterId = enemyCharacter.Id,
                CharacterName = enemyCharacter.Name,
                TargetCharacterId = playerCharacter.Id,
                TargetCharacterName = playerCharacter.Name,
            };

            if (enemyCharacter.CurrentHitPoints > 0)
            {
                var validSkills = enemyCharacter.Skills
                    .Where(s => s.TargetType == SkillTargetType.Enemy && s.EnergyCost <= enemyCharacter.CurrentEnergy && s.RemainingCooldown == 0)
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
            EndFight(fight, allEnemyCharactersInFight, playerCharacter);
        }

        resultDto.EnemyActions = enemyActions;
    }

    private static int CalculateSkillDamage(Skill skill, Character attacker, Character defender)
    {
        // TODO: Add some randomness
        var (damageBonus, damageReduction) = skill.DamageType switch
        {
            DamageType.Physical => (Math.Round((decimal)attacker.Strength / 100, 2), Math.Round((decimal)defender.Armor / 100, 2)),
            DamageType.Magic => (Math.Round((decimal)attacker.Intelligence / 100, 2), Math.Round((decimal)defender.Resistance / 100, 2)),
            _ => throw new ArgumentOutOfRangeException(nameof(skill.DamageType), "Invalid damage type")
        };

        var baseDamage = skill.Damage;
        var damageMultiplier = 1 + damageBonus - damageReduction;
        var damage = (int)Math.Round(baseDamage * damageMultiplier);

        return damage;
    }

    private static int CalculateWeaponDamage(Character attacker, Character defender)
    {
        // Weapons always deal physical damage
        // TODO: Add some randomness
        var damageBonus = Math.Round((decimal)attacker.Strength / 100, 2);
        var damageReduction = Math.Round((decimal)defender.Armor / 100, 2);

        var baseDamage = attacker.Weapon?.Damage ?? 1;
        var damageMultiplier = 1 + damageBonus - damageReduction;
        var damage = (int)Math.Round(baseDamage * damageMultiplier);

        return damage;
    }

    private static void RegenerateEnergy(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.CurrentEnergy += character.Spirit;
            if (character.CurrentEnergy > character.MaxEnergy) character.CurrentEnergy = character.MaxEnergy;
        }
    }

    private static void UpdateCooldowns(List<Character> allCharactersInFight)
    {
        foreach (var character in allCharactersInFight)
        {
            character.Skills.ForEach(s =>
            {
                if (s.RemainingCooldown > 0) s.RemainingCooldown--;
            });
        }
    }
}
