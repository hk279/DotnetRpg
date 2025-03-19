using DotnetRpg.Models.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class SkillEntityTypeConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasOne(s => s.StatusEffect).WithOne().HasForeignKey<Skill>(s => s.StatusEffectId);
        builder.ToTable("Skills");
    }
}