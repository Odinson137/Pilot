using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Data;
using Test.Base.IntegrationBase;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class BaseReceiverIntegrationTest : 
    IClassFixture<WorkerTestWorkerFactory>,
    IClassFixture<WorkerTestIdentityFactory>, 
    IClassFixture<WorkerTestStorageFactory>
{
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;

    protected readonly IPublishEndpoint PublishEndpoint;
    protected readonly HttpClient Client;
    protected readonly DataContext WorkerContext;
    
    protected readonly Pilot.Storage.Data.DataContext StorageContext;
    
    protected readonly IServiceScope WorkerScope;
    protected readonly IMapper WorkerMapper;
    protected DataContext AssertReceiverContext 
        => WorkerScope.ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseReceiverIntegrationTest(WorkerTestWorkerFactory workerTestWorkerFactory,
        WorkerTestIdentityFactory identityFactory, WorkerTestStorageFactory storageFactory)
    {
        WorkerScope = workerTestWorkerFactory.Services.CreateScope();
        WorkerContext = WorkerScope.ServiceProvider.GetRequiredService<DataContext>();
        Client = workerTestWorkerFactory.CreateClient();
        PublishEndpoint = WorkerScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        var identityClient = identityFactory.CreateClient();

        StorageContext = storageFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Storage.Data.DataContext>();
        var storageClient = storageFactory.CreateClient();

        WorkerMapper = WorkerScope.ServiceProvider.GetRequiredService<IMapper>();

        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityClient;
        HttpSingleTone.Init.HttpClients[ServiceName.StorageServer.ToString()] = storageClient;
    }
}