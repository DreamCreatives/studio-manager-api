using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioManager.Domain.Entities;

namespace StudioManager.Domain.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.EquipmentId).IsRequired();
        builder.HasIndex(x => x.EquipmentId, "IX_Reservations_EquipmentId");

        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.HasIndex(x => new { x.StartDate, x.EndDate }, "IX_Reservations_StartDate_EndDate");

        builder.HasOne(r => r.Equipment)
            .WithMany(e => e.Reservations)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(r => r.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
