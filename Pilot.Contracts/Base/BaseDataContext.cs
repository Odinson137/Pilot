using Microsoft.EntityFrameworkCore;

namespace Pilot.Contracts.Base;

public abstract class BaseDataContext : DbContext
{
    public BaseDataContext(DbContextOptions<BaseDataContext> options) : base(options)
    {
    }
}