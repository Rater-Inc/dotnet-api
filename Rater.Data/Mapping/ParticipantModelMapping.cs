using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rater.Domain.Models;

namespace Rater.Data.Mapping
{
    public class ParticipantModelMapping : IEntityTypeConfiguration<ParticipantModel>
    {
        public void Configure(EntityTypeBuilder<ParticipantModel> builder)
        {
            builder.HasKey(e => e.Id).HasName("participants_pkey");

            builder.ToTable("participants");

            builder.HasIndex(e => new { e.SpaceId, e.ParticipantName }, "participants_space_id_participant_name_key").IsUnique();

            builder.Property(e => e.Id).HasColumnName("participant_id");
            builder.Property(e => e.ParticipantName)
                .HasMaxLength(255)
                .HasColumnName("participant_name");
            builder.Property(e => e.SpaceId).HasColumnName("space_id");

            builder.HasOne(d => d.Space).WithMany(p => p.Participants)
                .HasForeignKey(d => d.SpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("participants_space_id_fkey");
        }
    }
}
