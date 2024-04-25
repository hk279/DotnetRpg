namespace DotnetRpg.Dtos.Item;

public class GetEquippedWeaponDto : GetEquippedItemDto
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
}
