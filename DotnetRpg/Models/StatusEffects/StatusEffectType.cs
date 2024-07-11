using System.Text.Json.Serialization;

namespace DotnetRpg.Models.StatusEffects;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusEffectType
{
    Unknown,
    Physical,
    Magic
}