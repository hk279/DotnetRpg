using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    private readonly Expression<Func<Item, bool>> _filterExpression;
    
    public ItemEntityTypeConfiguration(Expression<Func<Item, bool>> filterExpression)
    {
        _filterExpression = filterExpression;
    }
    
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ConfigureBaseEntity(_filterExpression);
        builder
            .HasDiscriminator(i => i.Type)
            .HasValue<ArmorPiece>(ItemType.ArmorPiece)
            .HasValue<Weapon>(ItemType.Weapon);
    }
}