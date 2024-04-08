using DotnetRpg.Dtos.Fight;
using DotnetRpg.Services.FightService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FightController : ControllerBase
{
    private readonly IFightService _fightService;

    public FightController(IFightService fightService)
    {
        _fightService = fightService;
    }

    [HttpPost("{characterId}")]
    public async Task<ActionResult<ServiceResponse<BeginFightResultDto>>> Fight(int characterId)
    {
        var response = await _fightService.BeginFight(characterId);
        return Ok(response);
    }

    [HttpPost("weapon-attack")]
    public async Task<ActionResult<ServiceResponse<PlayerActionResultDto>>> WeaponAttack(
        PlayerActionDto request
    )
    {
        var response = await _fightService.WeaponAttack(request);
        return Ok(response);
    }

    [HttpPost("use-skill")]
    public async Task<ActionResult<ServiceResponse<PlayerActionResultDto>>> UseSkill(
        PlayerSkillActionDto request
    )
    {
        var response = await _fightService.UseSkill(request);
        return Ok(response);
    }
}
