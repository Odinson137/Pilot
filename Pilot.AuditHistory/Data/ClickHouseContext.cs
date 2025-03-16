using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pilot.AuditHistory.Data;

public sealed class ClickHouseContext : DbContext
{
    public DbSet<Models.AuditHistory> AuditHistories { get; set; }
    
    public ClickHouseContext(DbContextOptions<ClickHouseContext> options) : base(options)
    {
        if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
        {
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypeBuilder = modelBuilder.Entity<Models.AuditHistory>();

        // уникальность в ClickHouse не требуется
        entityTypeBuilder.Ignore(e => e.Id);
        entityTypeBuilder.HasKey(e => new { e.EntityId, e.EntityType, e.CreateAt });

        entityTypeBuilder.ToTable("AuditHistory", table => table
            .HasMergeTreeEngine()
            .WithPrimaryKey("EntityId", "EntityType", "CreateAt"));
    }
}