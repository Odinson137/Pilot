using System.Net.Http.Json;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;
using File = Pilot.Storage.Models.File;

namespace Test.Storage.IntegrationTests;

public class FileTests(
    TestStorageFactory storageFactory,
    TestMessengerFactory messengerFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<File, FileDto>(testOutputHelper, ServiceName.StorageServer,
            configurations:
            [
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
                    HttpClient = storageFactory.CreateClient(),
                    IsMainService = true
                }
            ]),
        IClassFixture<TestStorageFactory>,
        IClassFixture<TestMessengerFactory>
{

    [Fact]
    public virtual async Task GetFileUrlTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values = GenerateTestEntity.CreateEntities<File>(count: count, listDepth: 0);

        foreach (var value in values)
            value.Name = Guid.NewGuid().ToString();

        var dataContext = GetContext(ServiceName.StorageServer);
        await dataContext.AddRangeAsync(values);
        await dataContext.SaveChangesAsync();

        var name = values.First().Name;

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(File)}/{Urls.FileUrl}/{name}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<FileDto>();
        Assert.NotNull(content);
        Assert.NotNull(content.Url);
        Assert.Equal(name, content.Name);
    }

    [Fact(Skip = "Работает по другому")]
    public override Task CreateModelTest_ReturnOk() => Task.CompletedTask;
    
    [Fact(Skip = "Работает по другому")]
    public override Task UpdateModelTest_ReturnOk() => Task.CompletedTask;
    
    [Fact(Skip = "Работает по другому")]
    public override Task DeleteModelTest_ReturnOk() => Task.CompletedTask;
}