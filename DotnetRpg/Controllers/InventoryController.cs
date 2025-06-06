using DotnetRpg.Dtos.Items;
using DotnetRpg.Services.InventoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase 
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("{characterId:int}")]
    public async Task<ActionResult<List<GetItemDto>>> GetInventory(int characterId)
    {
        var response = await _inventoryService.GetInventory(characterId);
        return Ok(response);
    }

    [HttpPost("{characterId:int}/equipItem/{itemId:int}")]
    public async Task<ActionResult> EquipItem(int characterId, int itemId)
    {
        await _inventoryService.EquipItem(characterId, itemId);
        return Ok();
    }
}
