using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pilot.Contracts.Models;

namespace Pilot.Receiver.Data;

public sealed class DataContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
        
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
        {
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            "server=pilot_mysql;user=root;password=12345678;database=PilotDb;",
            new MySqlServerVersion(new Version(8, 0, 11))
        );
    }
}