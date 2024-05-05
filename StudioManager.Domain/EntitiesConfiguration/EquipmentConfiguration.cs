using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public sealed class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name, "IX_Equipment_Name");
        
        builder.Property(x => x.Quantity).IsRequired();
        builder.HasIndex(x => x.Quantity, "IX_Equipment_Quantity");

        builder.Property(x => x.InitialQuantity).IsRequired();
        
        builder.Ignore(x => x.ImageUrl);
        
        builder.Property(x => x.EquipmentTypeId).IsRequired();
        builder.HasIndex(x => x.EquipmentTypeId, "IX_Equipment_EquipmentTypeId");
        
        builder.HasIndex(x => new{ x.Name, x.EquipmentTypeId }, "IX_Equipment_Name_EquipmentTypeId").IsUnique();

        builder.HasOne(e => e.EquipmentType)
            .WithMany(et => et.Equipments)
            .HasPrincipalKey(et => et.Id)
            .HasForeignKey(e => e.EquipmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}