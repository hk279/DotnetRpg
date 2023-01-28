namespace dotnet_rpg.Models
{
    public class Character
    {
        public Character(int stamina)
        {
            Stamina = stamina;
            MaxHitPoints = Stamina * 20;
            CurrentHitPoints = MaxHitPoints;
        }

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
        public User? User { get; set; }
        public Weapon? Weapon { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}