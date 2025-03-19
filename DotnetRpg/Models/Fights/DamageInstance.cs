using DotnetRpg.Models.Skills;

namespace DotnetRpg.Models.Fights;

public record DamageInstance(int TotalDamage, DamageType DamageType, HitType HitType);