namespace dotnet_rpg.Services.ItemService;

public interface IItemService
{
    public Item? GenerateFightRewardGear(Character playerCharacter, List<Character> enemies);
}
