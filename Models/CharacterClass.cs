using System.Text.Json.Serialization;

namespace dotnet_rpg.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CharacterClass
    {
        Warrior = 1,
        Mage = 2,
        Rogue = 3,
        Priest = 4
    }
}