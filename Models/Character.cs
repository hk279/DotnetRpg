namespace dotnet_rpg.Models
{
    public class Character
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
        public User? User { get; set; }
        public Weapon? Weapon { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}