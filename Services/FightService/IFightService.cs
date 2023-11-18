using dotnet_rpg.Dtos.Fight;

namespace dotnet_rpg.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<BeginFightResultDto>> BeginFight(int characterId);
    Task<ServiceResponse<PlayerActionResultDto>> WeaponAttack(PlayerActionDto request);
    Task<ServiceResponse<PlayerActionResultDto>> UseSkill(PlayerSkillActionDto request);
}
