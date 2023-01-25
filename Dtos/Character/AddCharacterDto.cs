namespace dotnet_rpg.Dtos.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = string.Empty;
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        public int Armor { get; set; } = 0;
        public int Resistance { get; set; } = 0;
        public CharacterClass Class { get; set; } = CharacterClass.Warrior;
    }
}