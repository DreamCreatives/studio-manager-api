using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioManager.Domain.Entities;

namespace StudioManager.Infrastructure.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.FirstName).IsRequired();

        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired();

        builder.Property(x => x.Email).IsRequired();
        builder.HasIndex(x => x.Email, "IX_Users_Email").IsUnique();
        
        builder.Property(x => x.KeycloakId).IsRequired();
        builder.HasIndex(x => x.KeycloakId, "IX_Users_KeycloakId").IsUnique();
    }
}
