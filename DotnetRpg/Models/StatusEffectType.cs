using System.Text.Json.Serialization;

namespace DotnetRpg.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusEffectType
{
    Unknown,
    Physical,
    Magic
}