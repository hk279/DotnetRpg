using DotnetRpg.Dtos.Item;
using DotnetRpg.Dtos.Skill;
using DotnetRpg.Dtos.StatusEffect;
using DotnetRpg.Models.Characters;

namespace DotnetRpg.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public CharacterClass Class { get; set; }
        public required string Avatar { get; set; }

        public int Level { get; set; }
        public long CurrentLevelTotalExperience { get; set; }
        public long ExperienceTowardsNextLevel { get; set; }

        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Stamina { get; set; }
        public int Spirit { get; set; }

        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }

        public int MaxEnergy { get; set; }
        public int CurrentEnergy { get; set; }

        public int Armor { get; set; }
        public int Resistance { get; set; }

        public int InventorySize { get; set; }
        public GetEquippedWeaponDto? EquippedWeapon { get; set; }
        public required List<GetEquippedArmorPieceDto> EquippedArmorPieces { get; set; }

        public required List<GetSkillInstanceDto> SkillInstances { get; set; }
        public required List<GetStatusEffectInstanceDto> StatusEffectInstances { get; set; }

        public int? FightId { get; set; }
    }
}
