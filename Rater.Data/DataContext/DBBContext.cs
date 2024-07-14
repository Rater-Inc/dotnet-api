global using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rater.Domain.Models;
using System.Reflection;

namespace Rater.Data.DataContext;

public partial class DBBContext : DbContext
{
    private readonly IConfiguration _config;
    public DBBContext(DbContextOptions<DBBContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    public virtual DbSet<MetricModel> Metrics { get; set; }

    public virtual DbSet<ParticipantModel> Participants { get; set; }

    public virtual DbSet<RatingModel> Ratings { get; set; }

    public virtual DbSet<SpaceModel> Spaces { get; set; }

    public virtual DbSet<UserModel> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _config.GetConnectionString("DefaultConnection");
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var models = ChangeTracker.Entries<BaseModel>();

        foreach (var model in models)
        {
            if (model.State == EntityState.Added)
            {
                model.Entity.CreatedDate = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
