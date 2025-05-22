using System.Net.Http.Json;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Worker.IntegrationTests;

public class CompanyTests(
    TestIdentityFactory identityFactory,
    TestStorageFactory storageFactory,
    TestWorkerFactory workerFactory,
    TestMessengerFactory messengerFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<Company, CompanyDto>(testOutputHelper, ServiceName.WorkerServer,
            configurations:
            [
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.IdentityServer,
                    ServiceProvider = identityFactory.Services,
                    DbContextType = typeof(Pilot.Identity.Data.DataContext),
                    HttpClient = identityFactory.CreateClient()
                },
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.WorkerServer,
                    ServiceProvider = workerFactory.Services,
                    DbContextType = typeof(Pilot.Worker.Data.DataContext),
                    HttpClient = workerFactory.CreateClient(),
                    IsMainService = true
                },
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.MessengerServer,
                    ServiceProvider = messengerFactory.Services,
                    DbContextType = typeof(Pilot.Messenger.Data.DataContext),
                    HttpClient = messengerFactory.CreateClient()
                },
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.StorageServer,
                    DbContextType = typeof(Pilot.Storage.Data.DataContext),
                    ServiceProvider = storageFactory.Services,
                    HttpClient = storageFactory.CreateClient()
                }
            ]),
        IClassFixture<TestIdentityFactory>,
        IClassFixture<TestStorageFactory>,
        IClassFixture<TestMessengerFactory>,
        IClassFixture<TestWorkerFactory>
{
    
    [Fact]
    public override async Task GetValueTest_ReturnOk()
    {
        #region Arrange

        const int count = 1;

        var values = GenerateTestEntity.CreateEntities<Company>(count: count, listDepth: 0);
        var projects = GenerateTestEntity.CreateEntities<Project>(count: count, listDepth: 0);
        values.First().Projects = projects;

        var workerContext = GetContext(ServiceName.WorkerServer);
        await workerContext.AddRangeAsync(values);
        await workerContext.SaveChangesAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{EntityName}/{id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<CompanyDto>();
        Assert.NotNull(content);
        Assert.Equal(id, content.Id);
    }
}