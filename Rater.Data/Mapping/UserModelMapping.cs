using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rater.Domain.Models;

namespace Rater.Data.Mapping
{
    public class UserModelMapping : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(e => e.Id).HasName("users_pkey");

            builder.ToTable("users");

            builder.Property(e => e.Id).HasColumnName("user_id");
            builder.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            builder.Property(e => e.Nickname)
                .HasMaxLength(255)
                .HasColumnName("nickname");
        }
    }
}
