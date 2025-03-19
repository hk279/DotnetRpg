using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Fights;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionType
{
    Unknown,
    Skill,
    WeaponAttack
}