using DotnetRpg.Models.Characters;

namespace DotnetRpg.Models.Generic;

public class CharacterSpecificEntity
{
    protected CharacterSpecificEntity() {}

    protected CharacterSpecificEntity(int characterId)
    {
        CharacterId = characterId;
    }

    public int Id { get; set; }
    public Character Character { get; set; } = null!;
    public int CharacterId { get; set; }
}