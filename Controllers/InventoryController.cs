using dotnet_rpg.Dtos.Item;
using dotnet_rpg.Services.ItemService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

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

    [HttpGet("{characterId}")]
    public async Task<ActionResult<ServiceResponse<List<GetItemDto>>>> GetInventory(int characterId)
    {
        var response = await _inventoryService.GetInventory(characterId);
        return Ok(response);
    }

    [HttpPost("{characterId}/equipItem/{itemId}")]
    public async Task<ActionResult> EquipItem(int characterId, int itemId)
    {
        await _inventoryService.EquipItem(characterId, itemId);
        return Ok();
    }
}
