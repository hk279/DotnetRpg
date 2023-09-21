using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.AutoMapper;

public class StrengthResolver : IValueResolver<Character, GetCharacterDto, int>
{
    public int Resolve(
        Character source,
        GetCharacterDto destination,
        int destMember,
        ResolutionContext context
    )
    {
        return source.GetStrength();
    }
}
