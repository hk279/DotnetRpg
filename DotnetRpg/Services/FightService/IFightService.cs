using DotnetRpg.Dtos.Fight;

namespace DotnetRpg.Services.FightService;

public interface IFightService
{
    Task<BeginFightResultDto> BeginFight(int characterId);
    Task<PlayerActionResultDto> WeaponAttack(PlayerActionDto request);
    Task<PlayerActionResultDto> UseSkill(PlayerSkillActionDto request);
}
