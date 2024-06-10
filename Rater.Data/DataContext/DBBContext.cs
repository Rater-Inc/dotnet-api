global using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Rater.API;

namespace Rater.Data.DataContext;

public partial class DBBContext : DbContext
{
    public DBBContext()
    {
    }

    private readonly IConfiguration _config;
    public DBBContext(DbContextOptions<DBBContext> options , IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    public virtual DbSet<Metric> Metrics { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Space> Spaces { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _config.GetSection("ConnectionStrings:DefaultConnection").Value;
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Metric>(entity =>
        {
            entity.HasKey(e => e.MetricId).HasName("metrics_pkey");

            entity.ToTable("metrics");

            entity.HasIndex(e => new { e.SpaceId, e.Name }, "metrics_space_id_name_key").IsUnique();

            entity.Property(e => e.MetricId).HasColumnName("metric_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.SpaceId).HasColumnName("space_id");

            entity.HasOne(d => d.Space).WithMany(p => p.Metrics)
                .HasForeignKey(d => d.SpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("metrics_space_id_fkey");
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("participants_pkey");

            entity.ToTable("participants");

            entity.HasIndex(e => new { e.SpaceId, e.ParticipantName }, "participants_space_id_participant_name_key").IsUnique();

            entity.Property(e => e.ParticipantId).HasColumnName("participant_id");
            entity.Property(e => e.ParticipantName)
                .HasMaxLength(255)
                .HasColumnName("participant_name");
            entity.Property(e => e.SpaceId).HasColumnName("space_id");

            entity.HasOne(d => d.Space).WithMany(p => p.Participants)
                .HasForeignKey(d => d.SpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("participants_space_id_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("ratings_pkey");

            entity.ToTable("ratings");

            entity.HasIndex(e => new { e.RaterId, e.RateeId, e.SpaceId, e.MetricId }, "ratings_rater_id_ratee_id_space_id_metric_id_key").IsUnique();

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.MetricId).HasColumnName("metric_id");
            entity.Property(e => e.RatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rated_at");
            entity.Property(e => e.RateeId).HasColumnName("ratee_id");
            entity.Property(e => e.RaterId).HasColumnName("rater_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.SpaceId).HasColumnName("space_id");

            entity.HasOne(d => d.Metric).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.MetricId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_metric_id_fkey");

            entity.HasOne(d => d.Ratee).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RateeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_ratee_id_fkey");

            entity.HasOne(d => d.Rater).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_rater_id_fkey");

            entity.HasOne(d => d.Space).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.SpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratings_space_id_fkey");
        });

        modelBuilder.Entity<Space>(entity =>
        {
            entity.HasKey(e => e.SpaceId).HasName("spaces_pkey");

            entity.ToTable("spaces");

            entity.Property(e => e.SpaceId).HasColumnName("space_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsLocked)
                .HasDefaultValue(false)
                .HasColumnName("is_locked");
            entity.Property(e => e.Link)
                .HasMaxLength(255)
                .HasColumnName("link");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");

            entity.HasOne(d => d.Creator).WithMany(p => p.Spaces)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("spaces_creator_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Nickname)
                .HasMaxLength(255)
                .HasColumnName("nickname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
