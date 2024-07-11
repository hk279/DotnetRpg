using DotnetRpg.Models.StatusEffects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class StatusEffectEntityTypeConfiguration : IEntityTypeConfiguration<StatusEffect>
{
    public void Configure(EntityTypeBuilder<StatusEffect> builder) { }
}