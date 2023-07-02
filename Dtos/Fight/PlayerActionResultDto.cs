using System.Text.Json.Serialization;

namespace dotnet_rpg.Dtos.Fight;

public class PlayerActionResultDto
{
    public required ActionDto PlayerAction { get; set; }
    public List<EnemyStatusDto> EnemyStatuses { get; set; } = new List<EnemyStatusDto>();
    public int PlayerCharacterRemainingHitPoints { get; set; }
    public FightStatus FightStatus { get; set; }
}

public class EnemyStatusDto
{
    public int EnemyCharacterId { get; set; }
    public required string EnemyCharacterName { get; set; }
    public required int EnemyRemainingHitPoints { get; set; }
    public ActionDto? EnemyAction { get; set; }
}

public class ActionDto
{
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