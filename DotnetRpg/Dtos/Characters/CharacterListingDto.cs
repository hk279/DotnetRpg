using DotnetRpg.Models.Characters;

namespace DotnetRpg.Dtos.Characters;

public record CharacterListingDto(int Id, string Name, int Level, string Avatar, CharacterClass Class)
{
    public static CharacterListingDto FromCharacter(Character character) =>
        new(character.Id, character.Name, character.Level, character.Avatar, character.Class);
}

