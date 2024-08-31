using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Models_File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Data;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
        {
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Models_File>().HasQueryFilter(c => c.DeleteAt == null);
    }

    public DbSet<Models_File> Files { get; set; }
}