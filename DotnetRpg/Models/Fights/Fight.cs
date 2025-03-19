using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Fights;

public class Fight : CharacterSpecificEntity
{
    public Fight() {}
    
    public Fight(int characterId, List<Character> allCharactersInFight)
    {
        CharacterId = characterId;
        AllCharactersInFight = allCharactersInFight;
    }
    
    public List<Character> AllCharactersInFight { get; set; } = null!;
}

