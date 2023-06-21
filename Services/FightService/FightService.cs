using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Fight;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService;

public class FightService : IFightService
{
    private readonly DataContext _context;

    public FightService(DataContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<BeginFightResultDto>> BeginFight(BeginFightDto request)
    {
        var response = new ServiceResponse<BeginFightResultDto>
        {
            Data = new BeginFightResultDto()
        };

        try
        {
            var playerCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.PlayerCharacterId);
            var enemies = await _context.Characters.Where(c => request.EnemyIds.Contains(c.Id)).ToListAsync();

            if (playerCharacter == null || enemies == null || enemies.Count == 0)
            {
                response.Success = false;
                response.Message = "Invalid fight. Player character or enemies not found.";
                return response;
            }

            var newFight = new Fight()
            {
                PlayerCharacter = playerCharacter,
                Enemies = enemies,
                FightStatus = FightStatus.Ongoing,
                IsPlayersTurn = true
            };

            await _context.AddAsync(newFight);
            await _context.SaveChangesAsync();

            response.Data = new BeginFightResultDto
            {
                Id = newFight.Id,
                PlayerCharacterId = playerCharacter.Id,
                EnemyIds = enemies.Select(c => c.Id).ToList()
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

            if (attacker == null || defender == null)
            {
                response.Success = false;
                response.Message = "Invalid attack. Attacker or defender not found.";
                return response;
            }

            var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

            if (skill == null)
            {
                response.Success = false;
                response.Message = "Invalid skill. Attacker doesn't possess this skill.";
                return response;
            }

            int damage = AttackWithSkill(attacker, defender, skill);

            if (defender.CurrentHitPoints <= 0)
            {
                response.Message = $"{defender.Name} has been defeated!";
            }

            await _context.SaveChangesAsync();

            response.Data = new AttackResultDto
            {
                AttackerName = attacker.Name,
                DefenderName = defender.Name,
                AttackerHitPoints = attacker.CurrentHitPoints,
                DefenderHitPoints = defender.CurrentHitPoints,
                Damage = damage
            };
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
    {
        var response = new ServiceResponse<AttackResultDto>();

        try
        {
            var attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
            var defender = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == request.DefenderId);

            if (attacker == null || defender == null)
            {
                response.Success = false;
                response.Message = "Invalid attack. Attacker or defender not found.";
                return response;
            }

            int damage = AttackWithWeapon(attacker, defender);

            if (defender.CurrentHitPoints <= 0)
            {
                response.Message = $"{defender.Name} has been defeated!";
            }

            await _context.SaveChangesAsync();

            response.Data = new AttackResultDto
            {
                AttackerName = attacker.Name,
                DefenderName = defender.Name,
                AttackerHitPoints = attacker.CurrentHitPoints,
                DefenderHitPoints = defender.CurrentHitPoints,
                Damage = damage
            };
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
        var (damageBonus, damageReduction) = skill.DamageType switch
        {
            DamageType.Physical => (Math.Round((decimal)attacker.Strength / 100, 2), Math.Round((decimal)defender.Armor / 100, 2)),
            DamageType.Magic => (Math.Round((decimal)attacker.Intelligence / 100, 2), Math.Round((decimal)defender.Resistance / 100, 2)),
            _ => throw new ArgumentOutOfRangeException(nameof(skill.DamageType), "Invalid damage type")
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
}
