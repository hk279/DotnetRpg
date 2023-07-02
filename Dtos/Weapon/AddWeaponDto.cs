namespace dotnet_rpg.Dtos.Weapon;

// TODO: Changes coming. Skills will be added through other events.
public class AddCharacterWeaponDto
{
    public required string Name { get; set; }
    public int Damage { get; set; }
    public int CharacterId { get; set; }
}
