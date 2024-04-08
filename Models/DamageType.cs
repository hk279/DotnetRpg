using System.Text.Json.Serialization;

namespace DotnetRpg.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    Unknown,
    Physical,
    Magic
}