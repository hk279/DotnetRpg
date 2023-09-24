using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.Services.FightService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

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
        AttackDto request
    )
    {
        var response = await _fightService.WeaponAttack(request);
        return Ok(response);
    }

    [HttpPost("skill-attack")]
    public async Task<ActionResult<ServiceResponse<PlayerActionResultDto>>> SkillAttack(
        SkillAttackDto request
    )
    {
        var response = await _fightService.SkillAttack(request);
        return Ok(response);
    }
}
