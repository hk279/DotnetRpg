namespace dotnet_rpg.Dtos.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = string.Empty;
        public int Strength { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        public int Stamina { get; set; } = 5;
        public CharacterClass Class { get; set; } = CharacterClass.Warrior;
    }
}