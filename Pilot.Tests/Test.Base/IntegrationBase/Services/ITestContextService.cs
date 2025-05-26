using Microsoft.EntityFrameworkCore;

namespace Test.Base.IntegrationBase.Services;

public interface ITestContextService
{
    void ResetDb();
    
    DbContext GetDbContext();
}