using dotnet_rpg.Dtos.Item;

namespace dotnet_rpg.Services.ItemService;

public interface IInventoryService
{
    Task<ServiceResponse<List<GetItemDto>>> GetInventory(int characterId);
    Task EquipItem(int characterId, int itemId);
    Task UnequipItem(int characterId, int itemId);
    Task DeleteItem(int characterId, int itemId);
    public Item? GenerateFightRewardGear(Character playerCharacter, List<Character> enemies);
}
