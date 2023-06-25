namespace dotnet_rpg.Dtos.Character;

public class GetCharacterListingDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public CharacterClass Class { get; set; }
}
