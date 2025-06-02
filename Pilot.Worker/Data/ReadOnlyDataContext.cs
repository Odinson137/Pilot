using Microsoft.EntityFrameworkCore;
using Pilot.Worker.Models;

namespace Pilot.Worker.Data;

public sealed class ReadOnlyDataContext : DbContext
{
    public ReadOnlyDataContext(DbContextOptions<ReadOnlyDataContext> options) : base(options)
    {
        // по логике это реплика и всё должно дублироваться с мастера
        // if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
        // {
        //     if (!databaseCreator.CanConnect()) databaseCreator.Create();
        //     if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        // }
    }

    // TODO придумать потом, чтоб не пришлось дублировать код как DataContext
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    
        builder.Entity<CompanyRole>()
            .HasMany(c => c.Companies)
            .WithMany(c => c.CompanyRoles);
    
        builder.Entity<ProjectTask>()
            .HasOne(pt => pt.TeamEmployee)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    
        builder.Entity<ProjectTask>()
            .HasOne(pt => pt.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    
        builder.Entity<CompanyUser>()
            .HasMany(cu => cu.Teams)
            .WithMany(t => t.CompanyUsers)
            .UsingEntity<TeamEmployee>(
                j => j
                    .HasOne(te => te.Team)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(te => te.CompanyUser)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade));
    
        builder.Entity<CompanyUser>()
            .HasOne(u => u.Company)
            .WithMany(c => c.CompanyUsers);
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyRole> CompanyRoles { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<TaskInfo> TaskInfos { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamEmployee> TeamEmployees { get; set; }
}