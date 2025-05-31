namespace DotnetRpg.Dtos.Characters;

public class AssignAttributePointsDto
{
    public int CharacterId { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Spirit { get; set; }
    public int Stamina { get; set; }
}