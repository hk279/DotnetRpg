using DotnetRpg.Dtos.Fight;
using DotnetRpg.Dtos.Fights;

namespace DotnetRpg.Services.FightService;

public interface IFightService
{
    Task<BeginFightResultDto> BeginFight(int playerCharacterId);
    Task<PlayerActionResultDto> WeaponAttack(PlayerActionDto request);
    Task<PlayerActionResultDto> UseSkill(PlayerSkillActionDto request);
}
