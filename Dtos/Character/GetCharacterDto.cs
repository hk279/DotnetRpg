using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.Weapon;

namespace dotnet_rpg.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Stamina { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public int Armor { get; set; }
        public int Resistance { get; set; }
        public CharacterClass? Class { get; set; }
        public GetWeaponDto? Weapon { get; set; }
        public List<GetSkillDto> Skills { get; set; } = new List<GetSkillDto>();
    }
}