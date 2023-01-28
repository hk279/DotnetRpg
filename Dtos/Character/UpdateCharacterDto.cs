namespace dotnet_rpg.Dtos.Character
{
    public class UpdateCharacterDto
    {
        // TODO: Change into "level up" request when expreience/levels are implemented
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Strength { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        public int Stamina { get; set; } = 5;
    }
}