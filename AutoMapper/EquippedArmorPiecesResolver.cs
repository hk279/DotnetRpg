using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Item;

namespace dotnet_rpg.AutoMapper;

public class EquippedArmorPiecesResolver
    : IValueResolver<Character, GetCharacterDto, List<GetEquippedArmorPieceDto>>
{
    public List<GetEquippedArmorPieceDto> Resolve(
        Character source,
        GetCharacterDto destination,
        List<GetEquippedArmorPieceDto> destMember,
        ResolutionContext context
    )
    {
        var equippedArmorPieces = source.Inventory
            .OfType<ArmorPiece>()
            .Where(item => item.Type == ItemType.ArmorPiece && item.IsEquipped)
            .Select(
                armorPiece =>
                    new GetEquippedArmorPieceDto
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
