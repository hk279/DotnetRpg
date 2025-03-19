using DotnetRpg.Models.Fights;

namespace DotnetRpg.Dtos.Fights;

public class ActionResultDto
{
    public int CharacterId { get; set; }
    public required string CharacterName { get; set; }
    public int TargetCharacterId { get; set; }
    public required string TargetCharacterName { get; set; }
    public ActionType ActionType { get; set; }
    public string? SkillName { get; set; }
    public DamageInstanceDto? DamageInstance { get; set; }
    public int Healing { get; set; }
}
