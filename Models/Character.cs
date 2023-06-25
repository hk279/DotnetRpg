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
        public bool IsPlayerCharacter { get; set; } = true;
        /// <summary>
        /// Bonus to physical damage type
        /// </summary>
        public int Strength { get; set; }
        /// <summary>
        /// Bonus to magic damage type
        /// </summary>
        public int Intelligence { get; set; }
        public int Stamina { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        /// <summary>
        /// Mitigation against physical damage type
        /// </summary>
        public int Armor { get; set; }
        /// <summary>
        /// Mitigation against magic damage type
        /// </summary>
        public int Resistance { get; set; }
        public CharacterClass Class { get; set; }
        public User? User { get; set; }
        public Weapon? Weapon { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public Fight? Fight { get; set; }
        public int? FightId { get; set; }
    }
}