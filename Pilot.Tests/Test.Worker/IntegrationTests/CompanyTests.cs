using System.Net.Http.Json;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class CompanyTests : BaseModelReceiverIntegrationTest<Company, CompanyDto>
{
    /// <inheritdoc />
    public CompanyTests(WorkerTestWorkerFactory workerTestWorkerFactory, WorkerTestIdentityFactory identityFactory,
        WorkerTestStorageFactory storageFactory,
        WorkerTestAuditHistoryFactory auditHistoryFactory
        ) :
        base(workerTestWorkerFactory, identityFactory, storageFactory, auditHistoryFactory)
    {
    }

    [Fact]
    public override async Task GetValue_ReturnOk()
    {
        #region Arrange

        const int count = 1;

        var values = GenerateTestEntity.CreateEntities<Company>(count: count, listDepth: 0);
        var projects = GenerateTestEntity.CreateEntities<Project>(count: count, listDepth: 0);
        values.First().Projects = projects;

        await WorkerContext.AddRangeAsync(values);
        await WorkerContext.SaveChangesAsync();

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

    // TODO потом просто сделать так, как я сделал в тестировании fileurl для всех моделей
    // protected override async ValueTask GetArrangeDop(ICollection<Company> values)
    // {
    //     foreach (var company in values)
    //     {
    //         company.LogoId = 1;
    //         company.InsideImagesId = [2];
    //     }
    // }
}