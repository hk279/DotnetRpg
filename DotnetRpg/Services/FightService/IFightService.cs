using DotnetRpg.Dtos.Fight;

namespace DotnetRpg.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<BeginFightResultDto>> BeginFight(int characterId);
    Task<ServiceResponse<PlayerActionResultDto>> WeaponAttack(PlayerActionDto request);
    Task<ServiceResponse<PlayerActionResultDto>> UseSkill(PlayerSkillActionDto request);
}
