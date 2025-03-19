using DotnetRpg.Models.Fights;
using DotnetRpg.Models.Skills;

namespace DotnetRpg.Dtos.Fights;

public record DamageInstanceDto(int TotalDamage, DamageType DamageType, HitType HitType)
{
    public static DamageInstanceDto FromDamageInstance(DamageInstance damageInstance)
        => new DamageInstanceDto(damageInstance.TotalDamage, damageInstance.DamageType, damageInstance.HitType);
}