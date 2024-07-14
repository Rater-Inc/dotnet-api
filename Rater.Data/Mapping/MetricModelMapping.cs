using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rater.Domain.Models;

namespace Rater.Data.Mapping;

public class MetricModelMapping : IEntityTypeConfiguration<MetricModel>
{
    public void Configure(EntityTypeBuilder<MetricModel> builder)
    {
        builder.HasKey(e => e.Id).HasName("metrics_pkey");

        builder.ToTable("metrics");

        builder.HasIndex(e => new { e.SpaceId, e.Name }, "metrics_space_id_name_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("metric_id");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Name)
           .HasMaxLength(255)
           .HasColumnName("name");
        builder.Property(e => e.SpaceId).HasColumnName("space_id");

        builder.HasOne(d => d.Space).WithMany(p => p.Metrics)
            .HasForeignKey(d => d.SpaceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("metrics_space_id_fkey");
    }
}
