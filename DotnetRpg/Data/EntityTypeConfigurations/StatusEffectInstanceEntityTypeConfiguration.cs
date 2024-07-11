using System.Linq.Expressions;
using DotnetRpg.Models.StatusEffects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class StatusEffectInstanceEntityTypeConfiguration : IEntityTypeConfiguration<StatusEffectInstance>
{
    private readonly Expression<Func<StatusEffectInstance, bool>> _filterExpression;
    
    public StatusEffectInstanceEntityTypeConfiguration(Expression<Func<StatusEffectInstance, bool>> filterExpression)
    {
        _filterExpression = filterExpression;
    }

    public void Configure(EntityTypeBuilder<StatusEffectInstance> builder)
    {
        builder.ConfigureBaseEntity(_filterExpression);
        builder.HasOne(i => i.StatusEffect).WithMany();
    }
}