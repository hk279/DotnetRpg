using System.Text.Json.Serialization;

namespace DotnetRpg.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CharacterClass
    {
        Unknown,
        Warrior,
        Mage,
        Priest
    }
}