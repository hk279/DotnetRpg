using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Items;

public record GetEquippedWeaponDto(
    int Id,
    string Name,
    ItemRarity Rarity,
    int Strength,
    int Intelligence,
    int Stamina,
    int Spirit,
    int MinDamage,
    int MaxDamage
    )
    : GetEquippedItemDto(Id, Name, Rarity, Strength, Intelligence, Stamina, Spirit)
{
    public static GetEquippedWeaponDto FromWeapon(Weapon weapon) =>
        new(
            weapon.Id,
            weapon.Name,
            weapon.Rarity,
            weapon.Strength,
            weapon.Intelligence,
            weapon.Stamina,
            weapon.Spirit,
            weapon.MinDamage,
            weapon.MaxDamage
        );
}
