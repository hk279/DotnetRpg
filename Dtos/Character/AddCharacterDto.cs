namespace dotnet_rpg.Dtos.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = string.Empty;
        public int HitPoints { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Defense { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public CharacterClass Class { get; set; } = CharacterClass.Warrior;
    }
}