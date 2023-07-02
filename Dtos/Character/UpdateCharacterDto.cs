namespace dotnet_rpg.Dtos.Character
{
    // Possibly redundant. Used for testing purposes for now. 
    public class UpdateCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Stamina { get; set; }
        public int Spirit { get; set; }
    }
}