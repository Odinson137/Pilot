using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Data;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class BaseReceiverIntegrationTest : 
    IClassFixture<WorkerTestWorkerFactory>,
    IClassFixture<WorkerTestIdentityFactory>, 
    IClassFixture<WorkerTestStorageFactory>,
    IClassFixture<WorkerTestAuditHistoryFactory>
{
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;

    protected readonly IPublishEndpoint PublishEndpoint;
    protected readonly HttpClient Client;
    protected readonly DataContext WorkerContext;
    
    protected readonly IServiceProvider WorkerService;
    protected readonly IMapper WorkerMapper;
    protected DataContext AssertReceiverContext 
        => WorkerService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseReceiverIntegrationTest(WorkerTestWorkerFactory workerTestWorkerFactory,
        WorkerTestIdentityFactory identityFactory, WorkerTestStorageFactory storageFactory,
        WorkerTestAuditHistoryFactory auditHistoryFactory)
    {
        WorkerService = workerTestWorkerFactory.Services;
        var workerScope = WorkerService.CreateScope();
        WorkerContext = workerScope.ServiceProvider.GetRequiredService<DataContext>();
        Client = workerTestWorkerFactory.CreateClient();
        PublishEndpoint = workerScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        var identityClient = identityFactory.CreateClient();

        WorkerMapper = workerScope.ServiceProvider.GetRequiredService<IMapper>();

        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityClient;
        HttpSingleTone.Init.HttpClients[ServiceName.StorageServer.ToString()] = storageFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.AuditHistoryService.ToString()] = auditHistoryFactory.CreateClient();
    }
}