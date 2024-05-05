using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public sealed class EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
{
    public void Configure(EntityTypeBuilder<EquipmentType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name, "IX_EquipmentType_Name").IsUnique();
        builder.Property(x => x.Name).IsRequired();
    }
}