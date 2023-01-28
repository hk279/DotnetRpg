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

    public async Task<ServiceResponse<FightResultDto>> Fight(BeginFightDto request)
    {
        var response = new ServiceResponse<FightResultDto>
        {
            Data = new FightResultDto()
        };

        try
        {
            var characters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => request.CharacterIds.Contains(c.Id))
                .ToListAsync();

            var defeated = false;

            var rng = new Random();
            characters = characters.OrderBy(a => rng.Next()).ToList();

            while (!defeated)
            {
                foreach (var attacker in characters)
                {
                    var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                    var opponent = opponents[rng.Next(opponents.Count)];

                    int damage = 0;
                    var fightLogRow = string.Empty;

                    var useWeapon = rng.Next(2) == 0;

                    if (useWeapon)
                    {
                        fightLogRow = $"{attacker.Name} attacked {opponent.Name} with {attacker.Weapon.Name}.";
                        damage = AttackWithWeapon(attacker, opponent);
                    }
                    else
                    {
                        var skill = attacker.Skills[rng.Next(attacker.Skills.Count)];
                        fightLogRow = $"{attacker.Name} used '{skill.Name}' on defender {opponent.Name}.";
                        damage = AttackWithSkill(attacker, opponent, skill);
                    }

                    fightLogRow += $" Damage inflicted: {damage}.";
                    response.Data.FightLog.Add(fightLogRow);

                    if (opponent.CurrentHitPoints <= 0)
                    {
                        defeated = true;
                        attacker.Victories++;
                        opponent.Defeats++;
                        response.Data.FightLog.Add($"{opponent.Name} has been defeated.");
                        response.Data.FightLog.Add($"{attacker.Name} wins with {attacker.CurrentHitPoints} HP left.");
                        break;
                    }
                }
            }
            characters.ForEach(c =>
            {
                c.Fights++;
                c.CurrentHitPoints = c.MaxHitPoints;
            });

            await _context.SaveChangesAsync();
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

    private static int AttackWithSkill(Character? attacker, Character? defender, Skill? skill)
    {
        // Magic damage scales with INT / RES
        // Physical damage scales with STR / ARM 
        var damageBonus = skill.DamageType == DamageType.Physical ?
            Math.Round((decimal)attacker.Strength / 100, 2) :
            Math.Round((decimal)attacker.Intelligence / 100, 2);

        var damageReduction = skill.DamageType == DamageType.Physical ?
            Math.Round((decimal)defender.Armor / 100, 2) :
            Math.Round((decimal)defender.Resistance / 100, 2);

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

    private static int AttackWithWeapon(Character? attacker, Character? defender)
    {
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
