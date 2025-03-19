using AutoMapper;
using DotnetRpg.Dtos.Characters;
using DotnetRpg.Dtos.Items;
using DotnetRpg.Dtos.Skills;
using DotnetRpg.Dtos.StatusEffects;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Items;
using DotnetRpg.Models.Skills;
using DotnetRpg.Models.StatusEffects;

namespace DotnetRpg.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddCharacterDto, Character>();
        CreateMap<Character, GetCharacterListingDto>();

        CreateMap<SkillInstance, GetSkillInstanceDto>();
        CreateMap<Skill, GetSkillDto>();

        CreateMap<StatusEffectInstance, GetStatusEffectInstanceDto>();
        CreateMap<StatusEffect, GetStatusEffectDto>();

        CreateMap<Weapon, GetItemDto>();
        CreateMap<ArmorPiece, GetItemDto>();
    }
}
