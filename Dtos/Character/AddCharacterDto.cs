namespace dotnet_rpg.Dtos.Character
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Stamina { get; set; }
        public int Spirit { get; set; }
        public CharacterClass? Class { get; set; }
    }
}