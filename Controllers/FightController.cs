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

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(BeginFightDto request)
    {
        return Ok(await _fightService.Fight(request));
    }

    [HttpPost("weapon-attack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
    {
        return Ok(await _fightService.WeaponAttack(request));
    }

    [HttpPost("skill-attack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
    {
        return Ok(await _fightService.SkillAttack(request));
    }
}
