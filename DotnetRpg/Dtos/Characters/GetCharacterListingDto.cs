using DotnetRpg.Models.Characters;

namespace DotnetRpg.Dtos.Characters;

public record GetCharacterListingDto(int Id, string Name, int Level, string Avatar, CharacterClass Class)
{
    public static GetCharacterListingDto FromCharacter(Character character) =>
        new(character.Id, character.Name, character.Level, character.Avatar, character.Class);
}

