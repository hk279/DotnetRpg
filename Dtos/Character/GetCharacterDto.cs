using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.Weapon;

namespace dotnet_rpg.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        public int Stamina { get; set; } = 5;
        public int Armor { get; set; } = 0;
        public int Resistance { get; set; } = 0;
        public CharacterClass? Class { get; set; }
        public GetWeaponDto? Weapon { get; set; }
        public List<GetSkillDto> Skills { get; set; } = new List<GetSkillDto>();
    }
}