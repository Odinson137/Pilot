using Microsoft.Extensions.DependencyInjection;

namespace Test.Identity.IntegrationTests;

public class BaseIdentityIntegrationTest : IClassFixture<IntegrationIdentityTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IServiceScope ScopeService;

    protected BaseIdentityIntegrationTest(IntegrationIdentityTestWebAppFactory factory)
    {
        ScopeService = factory.Services.CreateScope();

        // MongoDatabase = ScopeService.ServiceProvider.GetRequiredService<IMongoDatabase>();

        Client = factory.CreateClient();
    }
}