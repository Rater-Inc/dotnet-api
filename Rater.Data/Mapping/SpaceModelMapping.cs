using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rater.Domain.Models;

namespace Rater.Data.Mapping;

public class SpaceModelMapping : IEntityTypeConfiguration<SpaceModel>
{
    public void Configure(EntityTypeBuilder<SpaceModel> builder)
    {
        builder.HasKey(e => e.Id).HasName("spaces_pkey");

        builder.ToTable("spaces");

        builder.Property(e => e.Id).HasColumnName("space_id");
        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.CreatorId).HasColumnName("creator_id");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.IsLocked)
            .HasDefaultValue(false)
            .HasColumnName("is_locked");
        builder.Property(e => e.Link)
            .HasMaxLength(255)
            .HasColumnName("link");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
        builder.Property(e => e.Password)
            .HasMaxLength(255)
            .HasColumnName("password");

        builder.HasOne(d => d.Creator).WithMany(p => p.Spaces)
            .HasForeignKey(d => d.CreatorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("spaces_creator_id_fkey");
    }
}
