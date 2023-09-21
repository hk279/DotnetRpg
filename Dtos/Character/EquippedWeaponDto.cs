namespace dotnet_rpg.Dtos.Character;

public class EquippedWeaponDto : EquippedItemDto
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
}
