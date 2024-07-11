using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Skills;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SkillTargetType
{
    Unknown,
    Self,
    Friendly,
    Enemy,
    AllEnemies
}