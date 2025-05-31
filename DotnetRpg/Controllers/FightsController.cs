using DotnetRpg.Dtos.Fight;
using DotnetRpg.Dtos.Fights;
using DotnetRpg.Services.FightService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers;

[Authorize]
[ApiController]
[Route("fight")]
public class FightsController : ControllerBase
{
    private readonly IFightService _fightService;

    public FightsController(IFightService fightService)
    {
        _fightService = fightService;
    }

    [HttpPost]
    public async Task<ActionResult<BeginFightResultDto>> Fight(BeginFightDto request)
    {
        var response = await _fightService.BeginFight(request.PlayerCharacterId);
        return Ok(response);
    }

    [HttpPost("weapon-attack")]
    public async Task<ActionResult<PlayerActionResultDto>> WeaponAttack(PlayerActionDto request)
    {
        var response = await _fightService.WeaponAttack(request);
        return Ok(response);
    }

    [HttpPost("use-skill")]
    public async Task<ActionResult<PlayerActionResultDto>> UseSkill(PlayerSkillActionDto request)
    {
        var response = await _fightService.UseSkill(request);
        return Ok(response);
    }
}
