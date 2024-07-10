using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data;

public static class ModelBuilderExtensions
{
    public static void ConfigureBaseEntity<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, bool>> filterExpression
    )
        where TEntity : BaseEntity
    {
        builder.HasQueryFilter(filterExpression);
        builder.HasOne<User>().WithMany().HasForeignKey(e => e.UserId);
    }
}