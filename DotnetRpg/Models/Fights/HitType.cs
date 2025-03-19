using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Fights;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HitType
{
    Normal = 0,
    Weak = 1,
    Critical = 2
}