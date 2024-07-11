using System.Text.Json.Serialization;

namespace DotnetRpg.Models.Items;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemRarity
{
    Unknown = 0,
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4
}