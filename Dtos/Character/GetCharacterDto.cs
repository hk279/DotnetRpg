using dotnet_rpg.Dtos.Item;
using dotnet_rpg.Dtos.Skill;

namespace dotnet_rpg.Dtos.Character
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

        public required List<GetSkillDto> Skills { get; set; }

        public int? FightId { get; set; }
    }
}
