using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterListingDto>>>> GetAll()
    {
        var response = await _characterService.GetAllCharacters();

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("{characterId}/enemies")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetEnemies(
        int characterId
    )
    {
        var response = await _characterService.GetEnemies(characterId);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        var response = await _characterService.GetCharacterById(id);

        if (response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(await _characterService.GetCharacterById(id));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Add(
        AddCharacterDto newCharacter
    )
    {
        var response = await _characterService.AddCharacter(newCharacter);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
        var response = await _characterService.DeleteCharacter(id);

        if (response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    // TODO: Changes coming. Skills will be added through other events.
    [HttpPost("add-skill")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddSkill(
        AddCharacterSkillDto newCharacterSkill
    )
    {
        var response = await _characterService.AddCharacterSkill(newCharacterSkill);

        if (response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}
