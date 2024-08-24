using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Pilot.Receiver.Data;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class BaseReceiverIntegrationTest : IClassFixture<ReceiverTestReceiverFactory>,
    IClassFixture<ReceiverTestIdentityFactory>
{
    protected readonly DataContext AssertReceiverContext;
    protected readonly HttpClient IdentityClient;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;

    protected readonly IPublishEndpoint PublishEndpoint;
    protected readonly HttpClient ReceiverClient;
    protected readonly DataContext ReceiverContext;
    protected readonly IServiceScope ReceiverScope;
    protected IMapper ReceiverMapper;

    protected BaseReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory,
        ReceiverTestIdentityFactory identityFactory)
    {
        // Порядок важен для правильной инициализации подключений к сервисам

        ReceiverScope = receiverFactory.Services.CreateScope();
        ReceiverContext = ReceiverScope.ServiceProvider.GetRequiredService<DataContext>();
        AssertReceiverContext = ReceiverScope.ServiceProvider.GetRequiredService<DataContext>();
        ReceiverClient = receiverFactory.CreateClient();
        PublishEndpoint = ReceiverScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        IdentityClient = identityFactory.CreateClient();

        ReceiverMapper = ReceiverScope.ServiceProvider.GetRequiredService<IMapper>();

        HttpSingleTone.Init.HttpClients["IdentityServer"] = IdentityClient;
    }
}