using DotnetRpg.Dtos.Character;
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
    public async Task<ActionResult<List<GetCharacterListingDto>>> GetAll()
    {
        var response = await _characterService.GetAllCharacters();
        return Ok(response);
    }

    [HttpGet("{characterId:int}/enemies")]
    public async Task<ActionResult<List<GetCharacterDto>>> GetEnemies(
        int characterId
    )
    {
        var response = await _characterService.GetEnemies(characterId);
        return Ok(response);
    }

    [HttpGet("{characterId:int}")]
    public async Task<ActionResult<GetCharacterDto>> GetSingle(int characterId)
    {
        var response = await _characterService.GetCharacterById(characterId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> Add(AddCharacterDto newCharacter)
    {
        await _characterService.AddCharacter(newCharacter);
        return Ok();
    }

    [HttpDelete("{characterId:int}")]
    public async Task<ActionResult> Delete(int characterId)
    {
        await _characterService.DeleteCharacter(characterId);
        return Ok();
    }
}
