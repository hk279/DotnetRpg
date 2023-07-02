using dotnet_rpg.Dtos.Fight;

namespace dotnet_rpg.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<BeginFightResultDto>> BeginFight(BeginFightDto request);
    Task<ServiceResponse<PlayerActionResultDto>> WeaponAttack(AttackDto request);
    Task<ServiceResponse<PlayerActionResultDto>> SkillAttack(SkillAttackDto request);
}
