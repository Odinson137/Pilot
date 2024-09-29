using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Pilot.Worker.Data;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class BaseReceiverIntegrationTest : IClassFixture<WorkerTestWorkerFactory>,
    IClassFixture<WorkerTestIdentityFactory>
{
    protected readonly HttpClient IdentityClient;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;

    protected readonly IPublishEndpoint PublishEndpoint;
    protected readonly HttpClient Client;
    protected readonly DataContext WorkerContext;
    protected readonly IServiceScope WorkerScope;
    protected readonly IMapper WorkerMapper;
    protected DataContext AssertReceiverContext 
        => WorkerScope.ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseReceiverIntegrationTest(WorkerTestWorkerFactory workerTestWorkerFactory,
        WorkerTestIdentityFactory identityFactory)
    {
        WorkerScope = workerTestWorkerFactory.Services.CreateScope();
        WorkerContext = WorkerScope.ServiceProvider.GetRequiredService<DataContext>();
        Client = workerTestWorkerFactory.CreateClient();
        PublishEndpoint = WorkerScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        IdentityClient = identityFactory.CreateClient();

        WorkerMapper = WorkerScope.ServiceProvider.GetRequiredService<IMapper>();

        HttpSingleTone.Init.HttpClients["IdentityServer"] = IdentityClient;
    }
}