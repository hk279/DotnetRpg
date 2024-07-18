using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Items;

public record GetEquippedArmorPieceDto(
    int Id,
    string Name,
    ItemRarity Rarity,
    int Strength,
    int Intelligence,
    int Stamina,
    int Spirit,
    ArmorSlot Slot,
    int Armor,
    int Resistance) : GetEquippedItemDto(Id, Name, Rarity, Strength, Intelligence, Stamina, Spirit)
{
    public static GetEquippedArmorPieceDto FromArmorPiece(ArmorPiece armorPiece) =>
        new(
            armorPiece.Id,
            armorPiece.Name,
            armorPiece.Rarity,
            armorPiece.Strength,
            armorPiece.Intelligence,
            armorPiece.Stamina,
            armorPiece.Spirit,
            armorPiece.Slot,
            armorPiece.Armor,
            armorPiece.Resistance
        );
}

