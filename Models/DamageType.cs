using System.Text.Json.Serialization;

namespace dotnet_rpg.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    Unknown,
    Physical,
    Magic
}