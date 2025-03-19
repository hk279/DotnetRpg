using DotnetRpg.Models.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetRpg.Data.EntityTypeConfigurations;

public class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder
            .HasDiscriminator(i => i.Type)
            .HasValue<ArmorPiece>(ItemType.ArmorPiece)
            .HasValue<Weapon>(ItemType.Weapon);
        builder.ToTable("Items");
    }
}