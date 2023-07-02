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

    public async Task<ServiceResponse<BeginFightResultDto>> BeginFight(BeginFightDto request)
    {
        var response = new ServiceResponse<BeginFightResultDto>
        {
            Data = new BeginFightResultDto()
        };

        try
        {
            var allCharacters = new List<Character>();
            var playerCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.PlayerCharacterId)
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

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();

        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
            var defender = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.DefenderId);
            var fight = await _context.Fights.FindAsync(request.FightId);

            if (attacker == null || defender == null || fight == null) throw new Exception("Invalid attack");

            var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId) ?? throw new Exception("Invalid skill. Attacker doesn't possess this skill.");
            var damage = AttackWithSkill(attacker, defender, skill);
            var attackResult = new AttackResultDto
            {
                AttackerName = attacker.Name,
                DefenderName = defender.Name,
                AttackerHitPoints = attacker.CurrentHitPoints,
                DefenderHitPoints = defender.CurrentHitPoints,
                Damage = damage,
                FightStatus = FightStatus.Ongoing,
            };

            if (defender.CurrentHitPoints <= 0)
            {
                HandleEnemyDefeated(response, defender, fight, attackResult);
            }

            // TODO: Add defender action

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

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(AttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();

        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
            var defender = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.DefenderId);
            var fight = await _context.Fights.FindAsync(request.FightId);

            if (attacker == null || defender == null || fight == null) throw new Exception("Invalid attack");
            if (defender.CurrentHitPoints <= 0) throw new Exception("Defender is already dead");

            var damage = AttackWithWeapon(attacker, defender);
            var attackResult = new AttackResultDto
            {
                AttackerName = attacker.Name,
                DefenderName = defender.Name,
                AttackerHitPoints = attacker.CurrentHitPoints,
                DefenderHitPoints = defender.CurrentHitPoints,
                Damage = damage,
                FightStatus = FightStatus.Ongoing,
            };

            if (defender.CurrentHitPoints <= 0)
            {
                HandleEnemyDefeated(response, defender, fight, attackResult);
            }

            // TODO: Add defender action(s)

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
        var (damageBonus, damageReduction) = skill.SkillType switch
        {
            SkillType.Physical => (Math.Round((decimal)attacker.Strength / 100, 2), Math.Round((decimal)defender.Armor / 100, 2)),
            SkillType.Magic => (Math.Round((decimal)attacker.Intelligence / 100, 2), Math.Round((decimal)defender.Resistance / 100, 2)),
            _ => throw new ArgumentOutOfRangeException(nameof(skill.SkillType), "Invalid damage type")
        };

        var baseDamage = skill.Damage;
        var damageMultiplier = 1 + damageBonus - damageReduction;
        var damage = (int)Math.Round(baseDamage * damageMultiplier);

        // TODO: Add some randomness

        if (damage > 0)
        {
            defender.CurrentHitPoints -= damage;
        }

        return damage;
    }

    private static int AttackWithWeapon(Character attacker, Character defender)
    {
        // Weapons always deal physical damage
        var damageBonus = Math.Round((decimal)attacker.Strength / 100, 2);
        var damageReduction = Math.Round((decimal)defender.Armor / 100, 2);

        var baseDamage = attacker.Weapon?.Damage ?? 1;
        var damageMultiplier = 1 + damageBonus - damageReduction;
        var damage = (int)Math.Round(baseDamage * damageMultiplier);

        // TODO: Add some randomness

        if (damage > 0)
        {
            defender.CurrentHitPoints -= damage;
        }

        return damage;
    }

    private void HandleEnemyDefeated(ServiceResponse<AttackResultDto> response, Character defender, Fight fight, AttackResultDto attackResult)
    {
        response.Message = $"{defender.Name} has been defeated!";

        var allEnemyCharactersInFight = fight.Characters.Where(c => !c.IsPlayerCharacter);
        var allEnemiesDefeated = allEnemyCharactersInFight.All(c => c.CurrentHitPoints <= 0);

        if (allEnemiesDefeated) HandleVictory(fight, allEnemyCharactersInFight, attackResult);
    }

    private void HandleVictory(Fight fight, IEnumerable<Character> allEnemyCharactersInFight, AttackResultDto attackResult)
    {
        _context.RemoveRange(allEnemyCharactersInFight);
        _context.Remove(fight);
        attackResult.FightStatus = FightStatus.PlayerWon;
    }
}
