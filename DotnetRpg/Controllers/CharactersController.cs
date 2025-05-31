using DotnetRpg.Dtos.Characters;
using DotnetRpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CharactersController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharactersController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CharacterListingDto>>> GetAll()
    {
        var response = await _characterService.GetAllPlayerCharacters();
        return Ok(response);
    }

    [HttpGet("{characterId:int}/enemies")]
    public async Task<ActionResult<List<CharacterDto>>> GetEnemies(
        int characterId
    )
    {
        var response = await _characterService.GetEnemies(characterId);
        return Ok(response);
    }

    [HttpGet("{characterId:int}")]
    public async Task<ActionResult<CharacterDto>> GetSingle(int characterId)
    {
        var response = await _characterService.GetCharacterById(characterId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddCharacterDto addCharacterDto)
    {
        await _characterService.AddCharacter(addCharacterDto);
        return Ok();
    }

    [HttpDelete("{characterId:int}")]
    public async Task<ActionResult> Delete(int characterId)
    {
        await _characterService.DeleteCharacter(characterId);
        return Ok();
    }

    [HttpPost("assign-attribute-points")]
    public async Task<ActionResult> AssignAttributePoints(AssignAttributePointsDto request)
    {
        await _characterService.AssignAttributePoints(request);
        return Ok();
    }
}
