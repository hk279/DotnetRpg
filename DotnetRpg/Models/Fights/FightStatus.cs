using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Fights;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FightStatus
{
    Unknown,
    Ongoing,
    Victory,
    Defeat
}