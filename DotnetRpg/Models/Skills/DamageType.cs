using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Skills;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    Unknown,
    Physical,
    Magic
}