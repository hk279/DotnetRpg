using System.Security.Claims;
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
        return Ok(await _characterService.GetAllCharacters());
    }

    [HttpGet("{characterId}/enemies")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetEnemies(int characterId)
    {
        return Ok(await _characterService.GetEnemies(characterId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        var response = await _characterService.GetCharacterById(id);

        if (response.Data == null) return NotFound(response);

        return Ok(await _characterService.GetCharacterById(id));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Add(AddCharacterDto newCharacter)
    {
        return Ok(await _characterService.AddCharacter(newCharacter));
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Update(UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter);

        if (response.Data == null) return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
        var response = await _characterService.DeleteCharacter(id);

        if (response.Data == null) return NotFound(response);

        return Ok(response);
    }

    [HttpPost("add-skill")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var response = await _characterService.AddCharacterSkill(newCharacterSkill);

        if (response.Data == null) return NotFound(response);

        return Ok(response);
    }
}