using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Characters
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