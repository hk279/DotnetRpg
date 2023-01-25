namespace dotnet_rpg.Dtos.Character
{
    public class UpdateCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        public int Armor { get; set; } = 0;
        public int Resistance { get; set; } = 0;
        public CharacterClass Class { get; set; } = CharacterClass.Warrior;
    }
}