using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Skills;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    Unknown = 0,
    Physical = 1,
    Magic = 2
}