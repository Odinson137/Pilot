using System.Net.Http.Json;
using Pilot.Contracts.Data;
using Pilot.Contracts.DTO.ModelDto;
using Test.Base.IntegrationBase;
using Test.Storage.IntegrationTests.Factories;
using Xunit.Abstractions;
using File = Pilot.Storage.Models.File;

namespace Test.Storage.IntegrationTests;

public class FileTests(StorageTestStorageFactory factory, ITestOutputHelper testOutputHelper)
    : BaseModelIntegrationTest<File, FileDto>(factory, testOutputHelper)
{
    [Fact]
    public virtual async Task GetFileUrlsTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;
        var values = GenerateTestEntity.CreateEntities<File>(count: 2, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        #endregion
        
        // Act
        var result = await Client.GetAsync($"api/{nameof(File)}/{Urls.FileUrl}");
        
        TestOutputHelper.WriteLine(await result.Content.ReadAsStringAsync());
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<FileDto>>();
        Assert.NotNull(content);
        foreach (var fileDto in content)
            Assert.NotNull(fileDto.Url);
        Assert.True(content.Count >= count);
    }

    [Fact]
    public virtual async Task GetFileUrlTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values = GenerateTestEntity.CreateEntities<File>(count: count, listDepth: 0);

        await DataContext.AddRangeAsync(values);
        await DataContext.SaveChangesAsync();

        var id = values.First().Id;

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(File)}/{Urls.FileUrl}/{id}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<FileDto>();
        Assert.NotNull(content);
        Assert.NotNull(content.Url);
        Assert.Equal(id, content.Id);
    }

}