using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Storage.Data;
using Test.Storage.IntegrationTests.Factories;

namespace Test.Storage.IntegrationTests;

public class BaseStorageIntegrationTest : IClassFixture<FileTestStorageFactory>
{
    protected readonly DataContext DataContext;
    protected readonly HttpClient CapabilityClient;

    protected readonly IServiceScope StorageScope;
    protected DataContext AssertContext 
        => StorageScope.ServiceProvider.GetRequiredService<DataContext>();
    
    protected IMapper ReceiverMapper;

    
    protected BaseStorageIntegrationTest(FileTestStorageFactory factory)
    {
        StorageScope = factory.Services.CreateScope();
        DataContext = StorageScope.ServiceProvider.GetRequiredService<DataContext>();

        CapabilityClient = factory.CreateClient();

        ReceiverMapper = StorageScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}