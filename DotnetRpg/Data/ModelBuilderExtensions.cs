using System.Linq.Expressions;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;
using DotnetRpg.Models.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data;

public static class ModelBuilderExtensions
{
    public static void ConfigureUserSpecificEntity<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, bool>> filterExpression
    )
        where TEntity : UserSpecificEntity
    {
        builder.HasQueryFilter(filterExpression);
        builder.HasOne<User>().WithMany().HasForeignKey(e => e.UserId);
    }
}