using System.Text.Json.Serialization;

namespace DotnetRpg.Dtos.Fight;

public class PlayerActionResultDto
{
    public required ActionResultDto PlayerAction { get; set; }
    public List<ActionResultDto> EnemyActions { get; set; } = new List<ActionResultDto>();
    public FightStatus FightStatus { get; set; }
}

public class ActionResultDto
{
    public int CharacterId { get; set; }
    public required string CharacterName { get; set; }
    public int TargetCharacterId { get; set; }
    public required string TargetCharacterName { get; set; }
    public ActionType ActionType { get; set; }
    public string? SkillName { get; set; }
    public int Damage { get; set; }
    public int Healing { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionType
{
    Unknown,
    Skill,
    WeaponAttack
}
