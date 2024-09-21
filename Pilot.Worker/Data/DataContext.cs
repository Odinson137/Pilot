using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pilot.Worker.Models;

namespace Pilot.Worker.Data;

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
        base.OnModelCreating(builder);
        
        builder.Entity<CompanyRole>()
            .HasMany(c => c.Companies)
            .WithMany(c => c.CompanyRoles);
        
        builder.Entity<ProjectTask>()
            .HasOne(pt => pt.CompanyUser)
            .WithMany(cu => cu.ProjectTasks)
            .OnDelete(DeleteBehavior.Cascade);
    
        builder.Entity<ProjectTask>()
            .HasOne(pt => pt.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict); 
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Models.Company> Companies { get; set; }
    public DbSet<CompanyRole> CompanyRoles { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<TaskInfo> TaskInfos { get; set; }
    
    public DbSet<HistoryAction> HistoryActions { get; set; }
    public DbSet<Team> Teams { get; set; }
}