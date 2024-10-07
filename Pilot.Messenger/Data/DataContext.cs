using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Data;

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

    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Message>().HasQueryFilter(c => c.DeleteAt == null);
    }
}