using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class CharacterEntityTypeConfiguration : IEntityTypeConfiguration<Character>
{
    private readonly Expression<Func<Character, bool>> _filterExpression;
    
    public CharacterEntityTypeConfiguration(Expression<Func<Character, bool>> filterExpression)
    {
        _filterExpression = filterExpression;
    }

    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.ConfigureBaseEntity(_filterExpression);
        builder.HasMany(c => c.SkillInstances).WithOne(c => c.Character).OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(c => c.StatusEffectInstances).WithOne(c => c.Character).OnDelete(DeleteBehavior.NoAction);
    }
}