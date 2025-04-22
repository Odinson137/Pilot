using System.Net.Http.Json;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Test.Base.IntegrationBase;
using Test.Storage.IntegrationTests.Factories;
using Xunit.Abstractions;
using File = Pilot.Storage.Models.File;

namespace Test.Storage.IntegrationTests;

public class FileTests(StorageTestStorageFactory factory, ITestOutputHelper testOutputHelper)
    : BaseModelIntegrationTest<File, FileDto>(factory, testOutputHelper)
{
    [Fact]
    public virtual async Task GetFileUrlTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values = GenerateTestEntity.CreateEntities<File>(count: count, listDepth: 0);

        foreach (var value in values)
            value.Name = Guid.NewGuid().ToString();

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

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

}