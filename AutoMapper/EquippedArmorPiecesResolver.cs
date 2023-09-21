using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.AutoMapper;

public class EquippedArmorPiecesResolver
    : IValueResolver<Character, GetCharacterDto, List<EquippedArmorPieceDto>>
{
    public List<EquippedArmorPieceDto> Resolve(
        Character source,
        GetCharacterDto destination,
        List<EquippedArmorPieceDto> destMember,
        ResolutionContext context
    )
    {
        var equippedArmorPieces = source.Inventory
            .OfType<ArmorPiece>()
            .Where(item => item.Type == ItemType.ArmorPiece && item.IsEquipped)
            .Select(
                armorPiece =>
                    new EquippedArmorPieceDto
                    {
                        Id = armorPiece.Id,
                        Name = armorPiece.Name,
                        Rarity = armorPiece.Rarity,
                        Slot = armorPiece.Slot,
                        Armor = armorPiece.Armor,
                        Resistance = armorPiece.Resistance
                    }
            )
            .ToList();

        return equippedArmorPieces;
    }
}
