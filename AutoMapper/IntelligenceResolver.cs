using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.AutoMapper;

public class IntelligenceResolver : IValueResolver<Character, GetCharacterDto, int>
{
    public int Resolve(
        Character source,
        GetCharacterDto destination,
        int destMember,
        ResolutionContext context
    )
    {
        return source.GetIntelligence();
    }
}
