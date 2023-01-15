namespace dotnet_rpg.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int HitPoints { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Defense { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public CharacterClass Class { get; set; } = CharacterClass.Warrior;
        public User User { get; set; } = null!;
        public Weapon Weapon { get; set; } = null!;
        public List<Skill> Skills { get; set; } = null!;
    }
}