using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Item;

namespace dotnet_rpg.AutoMapper;

public class EquippedWeaponResolver
    : IValueResolver<Character, GetCharacterDto, GetEquippedWeaponDto?>
{
    public GetEquippedWeaponDto? Resolve(
        Character source,
        GetCharacterDto destination,
        GetEquippedWeaponDto? destMember,
        ResolutionContext context
    )
    {
        var equippedWeapon = source.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);

        if (equippedWeapon != null)
        {
            return new GetEquippedWeaponDto
            {
                Id = equippedWeapon.Id,
                Name = equippedWeapon.Name,
                Rarity = equippedWeapon.Rarity,
                MinDamage = equippedWeapon.MinDamage,
                MaxDamage = equippedWeapon.MaxDamage,
            };
        }

        return null;
    }
}
