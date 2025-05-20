using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Models;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Identity.Models;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Api.CapabilityService;

public class JobApplicationTests(
    TestApiFactory apiFactory,
    TestIdentityFactory identityFactory,
    TestStorageFactory storageFactory,
    TestWorkerFactory workerFactory,
    TestMessengerFactory messengerFactory,
    TestCapabilityFactory capabilityFactory,
    ITestOutputHelper testOutputHelper)
    : CapabilityTests<JobApplication, JobApplicationDto>(testOutputHelper, ServiceName.CapabilityServer,
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
    [Fact]
    public async Task UpdateValue_WithApprovedStatus_ReturnOk()
    {
        #region Arrange
    
        var user = await CreateUser();
    
        var type = typeof(JobApplication);
    
        var value = GenerateTestEntity.CreateEntities<JobApplication>(count: 1, listDepth: 0).First();
    
        value.AddCompanyUser(user.Id);
    
        var dbContext = GetContext(ServiceName.CapabilityServer);
        await dbContext.AddRangeAsync(value);
        await dbContext.SaveChangesAsync();
    
        var valueDto = Mapper.Map<JobApplicationDto>(value);
        valueDto.Status = ApplicationStatus.Approved;
    
        #endregion
    
        // Act
        var result = await ApiClient.PutAsJsonAsync($"api/{type.Name}", valueDto);
        await Helper.Wait();
    
        // Assert
        Assert.True(result.IsSuccessStatusCode);
        var content = await GetContext(ServiceName.CapabilityServer).Set<JobApplication>()
            .FirstOrDefaultAsync(c => c.Id == value.Id);
        Assert.NotNull(content);
        Assert.NotNull(content.ChangeAt);
    }

    private async Task<User> CreateUser(bool withAuthorization = true)
    {
        var user = GenerateTestEntity.CreateEntities<User>(count: 1).First();

        var context = GetContext(ServiceName.IdentityServer);
        await context.AddRangeAsync(user);
        await context.SaveChangesAsync();

        if (withAuthorization)
        {
            var token = TokenService.GenerateToken(user.Id, Role.Junior);
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return user;
    }
}