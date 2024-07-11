using DotnetRpg.Models.Characters;

namespace DotnetRpg.Dtos.Character;

public class GetCharacterListingDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; }
    public required string Avatar { get; set; }
    public CharacterClass Class { get; set; }
}
