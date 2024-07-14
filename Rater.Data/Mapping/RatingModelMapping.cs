using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rater.Domain.Models;

namespace Rater.Data.Mapping
{
    public class RatingModelMapping : IEntityTypeConfiguration<RatingModel>
    {
        public void Configure(EntityTypeBuilder<RatingModel> builder)
        {
            builder.HasKey(e => e.Id).HasName("ratings_pkey");

            builder.ToTable("ratings");

            builder.HasIndex(e => new { e.RaterId, e.RateeId, e.SpaceId, e.MetricId }, "ratings_rater_id_ratee_id_space_id_metric_id_key").IsUnique();

            builder.Property(e => e.Id).HasColumnName("rating_id");
            builder.Property(e => e.MetricId).HasColumnName("metric_id");
            builder.Property(e => e.RatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rated_at");
            builder.Property(e => e.RateeId).HasColumnName("ratee_id");
            builder.Property(e => e.RaterId).HasColumnName("rater_id");
            builder.Property(e => e.Score).HasColumnName("score");
            builder.Property(e => e.SpaceId).HasColumnName("space_id");

            builder.HasOne(d => d.Metric).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.MetricId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_metric_id_fkey");

            builder.HasOne(d => d.Ratee).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RateeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_ratee_id_fkey");

            builder.HasOne(d => d.Rater).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_rater_id_fkey");

            builder.HasOne(d => d.Space).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.SpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_space_id_fkey");
        }
    }
}
