using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Item;
using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.StatusEffect;

namespace dotnet_rpg.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDto>()
            .ForMember(
                dest => dest.EquippedWeapon,
                opt => opt.MapFrom(new EquippedWeaponResolver())
            )
            .ForMember(
                dest => dest.EquippedArmorPieces,
                opt => opt.MapFrom(new EquippedArmorPiecesResolver())
            )
            .ForMember(dest => dest.Strength, opt => opt.MapFrom(src => src.GetStrength()))
            .ForMember(dest => dest.Intelligence, opt => opt.MapFrom(src => src.GetIntelligence()))
            .ForMember(dest => dest.Stamina, opt => opt.MapFrom(src => src.GetStamina()))
            .ForMember(dest => dest.Spirit, opt => opt.MapFrom(src => src.GetSpirit()))
            .ForMember(dest => dest.Armor, opt => opt.MapFrom(src => src.GetArmor()))
            .ForMember(dest => dest.Resistance, opt => opt.MapFrom(src => src.GetResistance()))
            .ForMember(dest => dest.MaxEnergy, opt => opt.MapFrom(src => src.GetMaxEnergy()))
            .ForMember(dest => dest.MaxHitPoints, opt => opt.MapFrom(src => src.GetMaxHitPoints()))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.SkillInstances));
        CreateMap<Character, GetCharacterListingDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, Character>();
        CreateMap<SkillInstance, GetSkillInstanceDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Skill.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Skill.Description))
            .ForMember(dest => dest.DamageType, opt => opt.MapFrom(src => src.Skill.DamageType))
            .ForMember(dest => dest.TargetType, opt => opt.MapFrom(src => src.Skill.TargetType))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Skill.Rank))
            .ForMember(
                dest => dest.WeaponDamagePercentage,
                opt => opt.MapFrom(src => src.Skill.WeaponDamagePercentage)
            )
            .ForMember(dest => dest.EnergyCost, opt => opt.MapFrom(src => src.Skill.EnergyCost))
            .ForMember(dest => dest.Cooldown, opt => opt.MapFrom(src => src.Skill.Cooldown));
        CreateMap<StatusEffect, StatusEffectDto>();
        CreateMap<StatusEffectInstance, StatusEffectInstanceDto>();
        CreateMap<Weapon, GetItemDto>();
        CreateMap<ArmorPiece, GetItemDto>();
    }
}
