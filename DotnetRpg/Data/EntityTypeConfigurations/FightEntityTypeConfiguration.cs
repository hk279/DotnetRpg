using DotnetRpg.Models.Fights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class FightEntityTypeConfiguration : IEntityTypeConfiguration<Fight>
{
    public void Configure(EntityTypeBuilder<Fight> builder)
    {
        builder.HasMany(f => f.AllCharactersInFight).WithOne(c => c.Fight).OnDelete(DeleteBehavior.NoAction);
        builder.ToTable("Fights");
    }
}