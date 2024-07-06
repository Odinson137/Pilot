using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pilot.Contracts.Models;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Data;

public sealed class DataContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<HistoryAction> HistoryActions { get; set; }
    public DbSet<Team> Teams { get; set; }
        
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
        // optionsBuilder.UseMySql(
        //     "server=pilot_mysql;user=root;password=12345678;database=PilotDb;",
        //     new MySqlServerVersion(new Version(8, 0, 11))
        // );
    }
}