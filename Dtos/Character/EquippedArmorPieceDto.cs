namespace dotnet_rpg.Dtos.Character;

public class EquippedArmorPieceDto : EquippedItemDto
{
    public required ArmorSlot Slot { get; set; }
    public int Armor { get; set; }
    public int Resistance { get; set; }
}
