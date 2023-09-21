using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.AutoMapper;

public class EquippedWeaponResolver : IValueResolver<Character, GetCharacterDto, EquippedWeaponDto?>
{
    public EquippedWeaponDto? Resolve(
        Character source,
        GetCharacterDto destination,
        EquippedWeaponDto? destMember,
        ResolutionContext context
    )
    {
        var equippedWeapon = source.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);

        if (equippedWeapon != null)
        {
            return new EquippedWeaponDto
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
