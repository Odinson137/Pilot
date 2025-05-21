using Microsoft.EntityFrameworkCore;
using Pilot.Capability.Models;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Capability.IntegrationTests;

public class PostTests(
    TestIdentityFactory identityFactory,
    TestStorageFactory storageFactory,
    TestWorkerFactory workerFactory,
    TestMessengerFactory messengerFactory,
    TestCapabilityFactory capabilityFactory,
    ITestOutputHelper testOutputHelper)
    : BaseServiceModelTests<Post, PostDto>(testOutputHelper, ServiceName.CapabilityServer,
            configurations:
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
        IClassFixture<TestIdentityFactory>,
        IClassFixture<TestStorageFactory>,
        IClassFixture<TestMessengerFactory>,
        IClassFixture<TestWorkerFactory>,
        IClassFixture<TestCapabilityFactory>
{
    public override async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var value = GenerateTestEntity.CreateEntities<Post>(count: 1, listDepth: 0).First();

        var workerContext = GetContext(ServiceName.CapabilityServer);
        await workerContext.AddAsync(value);
        value.Skills = [new Skill {Title = Guid.NewGuid().ToString()}, new Skill {Title = Guid.NewGuid().ToString()}];
        
        var skills = new List<Skill> {new(){Title = Guid.NewGuid().ToString()}, new(){Title = Guid.NewGuid().ToString()}};
        await workerContext.AddRangeAsync(skills);
        await workerContext.SaveChangesAsync();

        var valueDto = Mapper.Map<PostDto>(value);

        valueDto.Skills = [new SkillDto {Id = skills[0].Id}, new SkillDto {Id = skills[1].Id}];
        
        #endregion

        // Act

        await Publisher.Publish(new UpdateCommandMessage<PostDto>(valueDto, 1));
        await Helper.Wait();

        // Assert

        var result = await GetContext(ServiceName.CapabilityServer).Set<Post>().Where(c => c.Id == value.Id).Include(c => c.Skills).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotNull(result.ChangeAt);
    }
}