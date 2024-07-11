using System.Linq.Expressions;
using DotnetRpg.Models.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class SkillInstanceEntityTypeConfiguration : IEntityTypeConfiguration<SkillInstance>
{
    private readonly Expression<Func<SkillInstance, bool>> _filterExpression;
    
    public SkillInstanceEntityTypeConfiguration(Expression<Func<SkillInstance, bool>> filterExpression)
    {
        _filterExpression = filterExpression;
    }
    
    public void Configure(EntityTypeBuilder<SkillInstance> builder)
    {
        builder.ConfigureBaseEntity(_filterExpression);
        builder.HasOne(i => i.Skill).WithMany();
    }
}