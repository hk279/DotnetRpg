using DotnetRpg.Models.StatusEffects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class StatusEffectInstanceEntityTypeConfiguration : IEntityTypeConfiguration<StatusEffectInstance>
{
    public void Configure(EntityTypeBuilder<StatusEffectInstance> builder)
    {
        builder.HasOne(i => i.StatusEffect).WithMany();
        builder.ToTable("StatusEffectInstances");
    }
}