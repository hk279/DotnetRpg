using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Dtos.Characters;

public record AddCharacterDto(
    string Name,
    string Avatar,
    int Strength,
    int Intelligence,
    int Stamina,
    int Spirit,
    CharacterClass CharacterClass)
{
    public Character ToCharacter(int userId) =>
        new(
            userId,
            Name,
            Avatar,
            CharacterClass,
            isPlayerCharacter: true,
            level: 1,
            new Attributes(Strength, Stamina, Intelligence, Spirit),
            baseArmor: 0,
            baseResistance: 0
        );
};