using AutoMapper;
using DotnetRpg.Data;
using DotnetRpg.Dtos.Characters;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Exceptions;
using DotnetRpg.Models.Generic;
using DotnetRpg.Models.Items;
using DotnetRpg.Models.Skills;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private const int InitialAttributePoints = 30;

    private readonly IMapper _autoMapper;
    private readonly DataContext _context;

    public CharacterService(IMapper autoMapper, DataContext context)
    {
        _autoMapper = autoMapper;
        _context = context;
    }

    public async Task<List<GetCharacterListingDto>> GetAllPlayerCharacters()
    {
        var allCharacters = await _context.Characters
            .Where(c => c.IsPlayerCharacter)
            .Select(c => _autoMapper.Map<GetCharacterListingDto>(c))
            .ToListAsync();

        return allCharacters;
    }

    public async Task<GetCharacterDto> GetCharacterById(int characterId)
    {
        var character =
            await _context.Characters
                .AsSplitQuery()
                .Where(c => c.IsPlayerCharacter)
                .Include(c => c.SkillInstances)
                .ThenInclude(s => s.Skill)
                .ThenInclude(s => s.StatusEffect)
                .Include(c => c.Inventory)
                .FirstOrDefaultAsync(c => c.Id == characterId)
            ?? throw new NotFoundException("Character not found");

        var dto = GetCharacterDto.FromCharacter(character);

        return dto;
    }

    public async Task<List<GetCharacterDto>> GetEnemies(int characterId)
    {
        var character = await _context.Characters.Include(c => c.Fight)
                            .FirstOrDefaultAsync(c => c.Id == characterId)
                        ?? throw new NotFoundException("Character not found");
        var fightId = character.Fight?.Id ?? throw new BadRequestException("Character is not in a fight");
        var enemies = await _context.Characters
            .Include(c => c.Fight)
            .Include(c => c.Inventory)
            .Include(c => c.StatusEffectInstances)
            .ThenInclude(s => s.StatusEffect)
            .Where(c => c.Fight != null && c.Fight.Id == fightId && !c.IsPlayerCharacter)
            .ToListAsync();
        var enemyDtos = enemies.Select(GetCharacterDto.FromCharacter).ToList();
        
        if (!enemies.Any())
        {
            throw new NotFoundException("No enemies found");
        }

        return enemyDtos;
    }

    public async Task AddCharacter(AddCharacterDto newCharacter)
    {
        var totalAttributePoints =
            newCharacter.Strength
            + newCharacter.Intelligence
            + newCharacter.Stamina
            + newCharacter.Spirit;

        if (totalAttributePoints is not InitialAttributePoints)
        {
            throw new BadRequestException("Invalid number of attribute points assigned");
        }

        var characterToAdd = newCharacter.ToCharacter(_context.UserId);

        ResetHitPointsAndEnergy(characterToAdd);
        AddStartingSkills(characterToAdd);
        AddStartingWeapon(characterToAdd);
        AddStartingGear(characterToAdd);

        await _context.Characters.AddAsync(characterToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCharacter(int characterId)
    {
        var characterToRemove = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId)
                                ?? throw new NotFoundException($"Character not found with ID {characterId}");

        _context.Characters.Remove(characterToRemove);

        await _context.SaveChangesAsync();
    }

    private static void ResetHitPointsAndEnergy(Character character)
    {
        character.CurrentHitPoints = character.GetMaxHitPoints();
        character.CurrentEnergy = character.GetMaxEnergy();
    }

    private void AddStartingSkills(Character character)
    {
        // TODO: Add other class starting skills
        switch (character.Class)
        {
            case CharacterClass.Warrior:
                var warriorStartingSkills = _context.Skills
                    .Include(s => s.StatusEffect)
                    .Where(
                        s =>
                            s.CharacterClass == CharacterClass.Warrior
                            && s.Rank == 1
                            && (s.Name == "Charge" || s.Name == "Rend")
                    )
                    .Select(s => new SkillInstance(character.Id, s, 0))
                    .ToList();

                character.SkillInstances.AddRange(warriorStartingSkills);

                return;
            case CharacterClass.Mage:
            case CharacterClass.Priest:
            default:
                return;
        }
    }

    private void AddStartingWeapon(Character character)
    {
        var weapon = character.Class switch
        {
            CharacterClass.Warrior
                => new Weapon(
                    characterId: character.Id,
                    name: "Training Shortsword",
                    weight: 2,
                    rarity: ItemRarity.Common,
                    value: 1,
                    level: 1,
                    minDamage: 4,
                    maxDamage: 5,
                    attributes: new Attributes(0, 0, 0, 0)
                ),
            CharacterClass.Mage
                => new Weapon(
                    characterId: character.Id,
                    name: "Handmade Dagger",
                    weight: 2,
                    rarity: ItemRarity.Common,
                    value: 1,
                    level: 1,
                    minDamage: 4,
                    maxDamage: 5,
                    attributes: new Attributes(0, 0, 0, 0)
                ),
            CharacterClass.Priest
                => new Weapon(
                    characterId: character.Id,
                    name: "Travelling Staff",
                    weight: 2,
                    rarity: ItemRarity.Common,
                    value: 1,
                    level: 1,
                    minDamage: 4,
                    maxDamage: 5,
                    attributes: new Attributes(0, 0, 0, 0)
                ),
            _ => throw new ArgumentException("Invalid character class")
        };

        weapon.IsEquipped = true;

        character.Inventory.Add(weapon);
    }

    private void AddStartingGear(Character character)
    {
        List<ArmorPiece> armorPieces = character.Class switch
        {
            CharacterClass.Warrior
                =>
                [
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Worn Leather Training Cuirass",
                        weight: 3,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Chest,
                        armor: 10,
                        resistance: 2,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Patched Leather Pants",
                        weight: 2,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Legs,
                        armor: 5,
                        resistance: 1,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Mudworn Boots",
                        weight: 1,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Feet,
                        armor: 3,
                        resistance: 1,
                        attributes: new Attributes(0, 0, 0, 0)
                    )
                ],
            CharacterClass.Mage
                =>
                [
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Worn Linen Tunic",
                        weight: 2,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Chest,
                        armor: 4,
                        resistance: 4,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Linen Leggings",
                        weight: 1,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Legs,
                        armor: 2,
                        resistance: 2,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Handmade Sandals",
                        weight: 1,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Feet,
                        armor: 1,
                        resistance: 1,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                ],
            CharacterClass.Priest
                => [
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Gray Novice Robe",
                        weight: 2,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Chest,
                        armor: 5,
                        resistance: 3,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Sackcloth Pants",
                        weight: 1,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Legs,
                        armor: 3,
                        resistance: 1,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                    new ArmorPiece(
                        characterId: character.Id,
                        name: "Sturdy Footwraps",
                        weight: 1,
                        rarity: ItemRarity.Common,
                        value: 1,
                        level: 1,
                        slot: ArmorSlot.Feet,
                        armor: 2,
                        resistance: 1,
                        attributes: new Attributes(0, 0, 0, 0)
                    ),
                ],
            _ => throw new ArgumentException("Invalid character class")
        };

        foreach (var armorPiece in armorPieces)
        {
            armorPiece.IsEquipped = true;
        }

        character.Inventory.AddRange(armorPieces);
    }
}
