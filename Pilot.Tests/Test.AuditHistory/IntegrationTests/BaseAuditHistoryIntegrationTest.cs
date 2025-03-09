using Microsoft.Extensions.DependencyInjection;
using Pilot.AuditHistory.Data;
using Test.AuditHistory.IntegrationTests.Factories;

namespace Test.AuditHistory.IntegrationTests;

public class BaseAuditHistoryIntegrationTest : IClassFixture<AuditHistoryTestAuditHistoryFactory>
{
    protected readonly ClickHouseContext DataContext;
    protected readonly HttpClient Client;

    protected readonly IServiceScope StorageScope;
    
    protected BaseAuditHistoryIntegrationTest(AuditHistoryTestAuditHistoryFactory factory)
    {
        StorageScope = factory.Services.CreateScope();
        DataContext = StorageScope.ServiceProvider.GetRequiredService<ClickHouseContext>();

        Client = factory.CreateClient();
    }
}