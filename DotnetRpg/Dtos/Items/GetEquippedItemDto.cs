using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Items;

public record GetEquippedItemDto(
    int Id,
    string Name,
    ItemRarity Rarity,
    int Strength,
    int Intelligence,
    int Stamina,
    int Spirit
);
