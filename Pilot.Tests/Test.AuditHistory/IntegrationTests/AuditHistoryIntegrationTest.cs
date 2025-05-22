using System.Net.Http.Json;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.AuditHistory.IntegrationTests;

[Collection(nameof(SequentialCollectionDefinition))]
public class AuditHistoryIntegrationTest(
    TestAuditHistoryFactory auditHistoryFactory,
    TestWorkerFactory workerFactory,
    TestMessengerFactory messengerFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<Pilot.AuditHistory.Models.AuditHistory, AuditHistoryDto>(testOutputHelper,
            ServiceName.AuditHistoryService,
            configurations:
            [
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.WorkerServer,
                    ServiceProvider = workerFactory.Services,
                    DbContextType = typeof(Pilot.Worker.Data.DataContext),
                    HttpClient = workerFactory.CreateClient()
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
                    ServiceName = ServiceName.AuditHistoryService,
                    DbContextType = typeof(Pilot.AuditHistory.Data.ClickHouseContext),
                    ServiceProvider = auditHistoryFactory.Services,
                    HttpClient = auditHistoryFactory.CreateClient(),
                    IsMainService = true
                }
            ]),
        IClassFixture<TestMessengerFactory>,
        IClassFixture<TestWorkerFactory>,
        IClassFixture<TestAuditHistoryFactory>
{
    [Fact]
    public override async Task GetValueTest_ReturnOk()
    {
        #region Arrange

        const int count = 2;

        var values =
            GenerateTestEntity.CreateEntities<Pilot.AuditHistory.Models.AuditHistory>(count: count, listDepth: 0);

        var dataContext = GetContext(ServiceName.AuditHistoryService);
        await dataContext.AddRangeAsync(values);
        await dataContext.SaveChangesAsync();

        var filter = new BaseFilter
        {
            Skip = 0,
            Take = 1,
            WhereFilter = new WhereFilter((nameof(AuditHistoryDto.EntityId), values.First().EntityId),
                (nameof(AuditHistoryDto.EntityType), ModelType.ProjectTask))
        };

        #endregion

        // Act
        var result = await Client.GetAsync($"api/{nameof(Pilot.AuditHistory.Models.AuditHistory)}?filter={filter.ToJson()}");

        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<AuditHistoryDto>>();
        Assert.NotNull(content);
        Assert.Single(content);
    }

    [Fact(Skip = "Работает по другому")]
    public override Task CreateModelTest_ReturnOk() => Task.CompletedTask;

    [Fact(Skip = "Работает по другому")]
    public override Task UpdateModelTest_ReturnOk() => Task.CompletedTask;

    [Fact(Skip = "Работает по другому")]
    public override Task DeleteModelTest_ReturnOk() => Task.CompletedTask;
}