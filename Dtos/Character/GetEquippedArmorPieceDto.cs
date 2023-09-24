namespace dotnet_rpg.Dtos.Character;

public class GetEquippedArmorPieceDto : GetEquippedItemDto
{
    public required ArmorSlot Slot { get; set; }
    public int Armor { get; set; }
    public int Resistance { get; set; }
}
