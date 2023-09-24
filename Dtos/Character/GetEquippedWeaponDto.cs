namespace dotnet_rpg.Dtos.Character;

public class GetEquippedWeaponDto : GetEquippedItemDto
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
}
