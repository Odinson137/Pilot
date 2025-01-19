using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pilot.Capability.Models;

namespace Pilot.Capability.Data;

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

    public DbSet<Post> Posts { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<CompanyPost> CompanyPosts { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CompanyPost>()
            .HasMany(c => c.Applications)
            .WithOne(c => c.CompanyPost);
    }
}