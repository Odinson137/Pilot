using System.Net.Http.Json;
using Pilot.Capability.Models;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Api.CapabilityService;

public class UserSkillTests(
    TestApiFactory apiFactory,
    TestIdentityFactory identityFactory,
    TestStorageFactory storageFactory,
    TestWorkerFactory workerFactory,
    TestMessengerFactory messengerFactory,
    TestCapabilityFactory capabilityFactory,
    ITestOutputHelper testOutputHelper)
    : CapabilityTests<UserSkill, UserSkillDto>(testOutputHelper, ServiceName.CapabilityServer,
            apiConfiguration: new ServiceTestConfiguration
            {
                ServiceName = ServiceName.ApiServer,
                ServiceProvider = apiFactory.Services,
                HttpClient = apiFactory.CreateClient()
            }, configurations:
            [
                new ServiceTestConfiguration
                {
                    ServiceName = ServiceName.CapabilityServer,
                    ServiceProvider = capabilityFactory.Services,
                    DbContextType = typeof(Pilot.Capability.Data.DataContext),
                    HttpClient = capabilityFactory.CreateClient(),
                    IsMainService = true
                },
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
                    ServiceName = ServiceName.StorageServer,
                    DbContextType = typeof(Pilot.Storage.Data.DataContext),
                    ServiceProvider = storageFactory.Services,
                    HttpClient = storageFactory.CreateClient()
                }
            ]),
        IClassFixture<TestApiFactory>,
        IClassFixture<TestIdentityFactory>,
        IClassFixture<TestStorageFactory>,
        IClassFixture<TestMessengerFactory>,
        IClassFixture<TestWorkerFactory>,
        IClassFixture<TestCapabilityFactory>
{
    private readonly ITestOutputHelper _testOutputHelper1 = testOutputHelper;

    [Fact]
    public virtual async Task GetUserSkillTest_ReturnOk()
    {
        #region Arrange
    
        const int count = 1;
        var skill = GenerateTestEntity.CreateEntities<Skill>(count: count, listDepth: 0).First();

        var dbContext = GetContext(ServiceName.CapabilityServer);
        await dbContext.AddRangeAsync(skill);
        
        var userSkill = GenerateTestEntity.CreateEntities<UserSkill>(count: count, listDepth: 0).First();
        
        await dbContext.AddRangeAsync(userSkill);

        userSkill.Skill = skill;
        userSkill.UserId = 1;
        
        await dbContext.SaveChangesAsync();
        
        #endregion
    
        // Act
        var result = await Client.GetAsync($"api/{EntityName}/{Urls.UserSkills}/{userSkill.UserId}");
        _testOutputHelper1.WriteLine($"Status Code: {result.StatusCode}");
        _testOutputHelper1.WriteLine($"Response: {await result.Content.ReadAsStringAsync()}");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await result.Content.ReadFromJsonAsync<ICollection<SkillDto>>();
        Assert.NotNull(content);
        Assert.True(content.Count >= count);
    }
}