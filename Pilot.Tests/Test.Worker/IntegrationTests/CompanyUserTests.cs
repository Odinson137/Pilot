using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;
using Pilot.Worker.Models;
using Test.Base.IntegrationBase;
using Test.Base.IntegrationBase.Factories;
using Xunit.Abstractions;

namespace Test.Worker.IntegrationTests;

public class CompanyUserTests(
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
    public override async Task UpdateModelTest_ReturnOk()
    {
        #region Arrange

        var companyUser = await CreateUser();

        var valueDto = Mapper.Map<CompanyUserDto>(companyUser);

        #endregion

        // Act

        await Publisher.Publish(new UpdateCommandMessage<CompanyUserDto>(valueDto, companyUser.Id));
        await Helper.Wait();

        // Assert

        var result = await GetContext(ServiceName.WorkerServer).Set<CompanyUser>().Where(c => c.Id == companyUser.Id)
            .FirstOrDefaultAsync();

        Assert.NotNull(result);
    }
}