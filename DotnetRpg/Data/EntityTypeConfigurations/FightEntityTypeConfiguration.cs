using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class FightEntityTypeConfiguration : IEntityTypeConfiguration<Fight>
{
    private readonly Expression<Func<Fight, bool>> _filterExpression;
    
    public FightEntityTypeConfiguration(Expression<Func<Fight, bool>> filterExpression)
    {
        _filterExpression = filterExpression;
    }
    
    public void Configure(EntityTypeBuilder<Fight> builder)
    {
        builder.ConfigureBaseEntity(_filterExpression);
        builder.HasMany(f => f.Characters).WithOne(c => c.Fight).OnDelete(DeleteBehavior.SetNull);
    }
}