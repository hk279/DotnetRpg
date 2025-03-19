using DotnetRpg.Models.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class SkillInstanceEntityTypeConfiguration : IEntityTypeConfiguration<SkillInstance>
{
    public void Configure(EntityTypeBuilder<SkillInstance> builder)
    {
        builder.HasOne(i => i.Skill).WithMany();
        builder.ToTable("SkillInstances");
    }
}