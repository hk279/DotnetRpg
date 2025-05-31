using DotnetRpg.Dtos.Items;
using DotnetRpg.Dtos.Skills;
using DotnetRpg.Dtos.StatusEffects;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Items;

namespace DotnetRpg.Dtos.Characters;

public record CharacterDto(
    int Id,
    string Name,
    CharacterClass Class,
    string Avatar,
    int Level,
    int NextLevelExperienceThreshold,
    int CurrentLevelExperienceGained,
    int UnassignedAttributePoints,
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
    public static CharacterDto FromCharacter(Character character)
    {
        var currentLevelExperienceThreshold = LevelExperienceThresholds.AllThresholds.GetValueOrDefault(character.Level);
        var nextLevelExperienceThreshold = LevelExperienceThresholds.AllThresholds.GetValueOrDefault(character.Level + 1);

        // Total amount of experience needed to progress through the current level
        var currentLevelTotalExperience = nextLevelExperienceThreshold - currentLevelExperienceThreshold;
        // The amount of experience gained so far towards the next level up
        var currentLevelExperienceGained = character.Experience - currentLevelExperienceThreshold;

        var equippedWeapon = character.Inventory
            .OfType<Weapon>()
            .SingleOrDefault(item => item is { Type: ItemType.Weapon, IsEquipped: true });
        var equippedArmorPieces = character.Inventory
            .OfType<ArmorPiece>()
            .Where(item => item is { Type: ItemType.ArmorPiece, IsEquipped: true })
            .Select(GetEquippedArmorPieceDto.FromArmorPiece).ToList();
        var skills = character.SkillInstances
            .Select(si => new GetSkillInstanceDto(si.RemainingCooldown, GetSkillDto.FromSkill(si.Skill)))
            .ToList();
        var statusEffects = character.StatusEffectInstances
            .Select(GetStatusEffectInstanceDto.FromStatusEffectInstance)
            .ToList();
        
        return new CharacterDto(
            character.Id,
            character.Name,
            character.Class,
            character.Avatar,
            character.Level,
            currentLevelTotalExperience,
            currentLevelExperienceGained,
            character.UnassignedAttributePoints,
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
            equippedArmorPieces,
            skills,
            statusEffects,
            character.Fight?.Id
        );
    }
};
