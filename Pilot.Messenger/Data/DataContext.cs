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

    public DbSet<InfoMessage> InfoMessages { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<InfoMessage>().HasQueryFilter(c => c.DeleteAt == null);
        builder.Entity<Message>().HasQueryFilter(c => c.DeleteAt == null);
        builder.Entity<Chat>().HasQueryFilter(c => c.DeleteAt == null);
        builder.Entity<ChatMember>().HasQueryFilter(c => c.DeleteAt == null);
    }
}