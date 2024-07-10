using DotnetRpg.Dtos.Item;

namespace DotnetRpg.Services.InventoryService;

public interface IInventoryService
{
    Task<List<GetItemDto>> GetInventory(int characterId);
    Task EquipItem(int characterId, int itemId);
    Task UnequipItem(int characterId, int itemId);
    Task DeleteItem(int characterId, int itemId);
    public Item? GenerateFightRewardGear(Character playerCharacter, List<Character> enemies);
}
