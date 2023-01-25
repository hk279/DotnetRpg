using System.Text.Json.Serialization;

namespace dotnet_rpg.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    Physical = 1,
    Magic = 2
}