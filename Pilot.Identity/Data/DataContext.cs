using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;
using Pilot.Identity.Models;

namespace Pilot.Identity.Data;

public sealed class DataContext : BaseDataContext
{
    public DbSet<User> Users { get; set; }

    public DataContext()
    {
        Database.EnsureCreated();
    }
}