using System.Text.Json.Serialization;

namespace dotnet_rpg.Dtos.Fight;

public class PlayerActionResultDto
{
    public required ActionDto PlayerAction { get; set; }
    public List<ActionDto> EnemyActions { get; set; } = new List<ActionDto>();
    public FightStatus FightStatus { get; set; }
}

public class ActionDto
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