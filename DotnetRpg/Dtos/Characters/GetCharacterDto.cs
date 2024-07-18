using DotnetRpg.Dtos.Items;
using DotnetRpg.Dtos.Skills;
using DotnetRpg.Dtos.StatusEffects;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Characters;

public record GetCharacterDto(
    int Id,
    string Name,
    CharacterClass Class,
    string Avatar,
    int Level,
    int CurrentLevelTotalExperience,
    int CurrentLevelExperienceGained,
    int Strength,
    int Intelligence,
    int Stamina,
    int Spirit,
    int MaxHitPoints,
    int CurrentHitPoints,
    int MaxEnergy,
    int CurrentEnergy,
    int Armor,
    int Resistance,
    int InventorySize,
    GetEquippedWeaponDto? EquippedWeapon,
    List<GetEquippedArmorPieceDto> EquippedArmorPieces,
    List<GetSkillInstanceDto> SkillInstances,
    List<GetStatusEffectInstanceDto> StatusEffectInstances,
    int? FightId)
{
    public static GetCharacterDto FromCharacter(Character character)
    {
        var currentLevelExperienceThreshold = LevelExperienceThresholds.AllThresholds.GetValueOrDefault(character.Level);
        var nextLevelExperienceThreshold = LevelExperienceThresholds.AllThresholds.GetValueOrDefault(character.Level + 1);

        // Total amount of experience needed to progress through the current level
        var currentLevelTotalExperience = nextLevelExperienceThreshold - currentLevelExperienceThreshold;
        // The amount of experience gained so far towards the next level up
        var currentLevelExperienceGained = character.Experience - currentLevelExperienceThreshold;

        var equippedWeapon = character.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item.Type == ItemType.Weapon && item.IsEquipped);
        var equippedArmorPieces = character.Inventory
            .OfType<ArmorPiece>()
            .Where(item => item.Type == ItemType.ArmorPiece && item.IsEquipped)


        return new(
            character.Id,
            character.Name,
            character.Class,
            character.Avatar,
            character.Level,
            currentLevelTotalExperience,
            currentLevelExperienceGained,
            character.GetStrength(),
            character.GetIntelligence(),
            character.GetStamina(),
            character.GetSpirit(),
            character.GetMaxHitPoints(),
            character.CurrentHitPoints,
            character.GetMaxEnergy(),
            character.CurrentEnergy,
            character.GetArmor(),
            character.GetResistance(),
            character.InventorySize,
            equippedWeapon is not null ? GetEquippedWeaponDto.FromWeapon(equippedWeapon) : null,
            equippedArmorPieces.Select(GetEquippedArmorPieceDto.FromArmorPiece).ToList(),


        )
    }
};
