using Microsoft.EntityFrameworkCore;

namespace Test.Base.IntegrationBase.Services;

public class TestContextService(DbContext dbContext) : ITestContextService
{
    public void ResetDb()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    public DbContext GetDbContext()
    {
        return dbContext;
    }
}