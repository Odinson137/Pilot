using Microsoft.Extensions.DependencyInjection;
using Pilot.Storage.Data;
using Test.Storage.IntegrationTests.Factories;

namespace Test.Storage.IntegrationTests;

public class BaseStorageIntegrationTest : IClassFixture<StorageTestStorageFactory>
{
    protected readonly DataContext DataContext;
    protected readonly HttpClient Client;

    protected readonly IServiceScope StorageScope;
    protected DataContext AssertContext 
        => StorageScope.ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseStorageIntegrationTest(StorageTestStorageFactory factory)
    {
        StorageScope = factory.Services.CreateScope();
        DataContext = StorageScope.ServiceProvider.GetRequiredService<DataContext>();

        Client = factory.CreateClient();
    }
}