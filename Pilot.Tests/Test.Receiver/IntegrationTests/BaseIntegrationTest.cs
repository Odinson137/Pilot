using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Test.Receiver.IntegrationTests.Factories;

namespace Test.Receiver.IntegrationTests;

public class BaseReceiverIntegrationTest : IClassFixture<ReceiverTestReceiverFactory>, IClassFixture<ReceiverTestIdentityFactory>
{
    protected readonly HttpClient ReceiverClient;
    protected readonly HttpClient IdentityClient;
    protected readonly Pilot.Receiver.Data.DataContext ReceiverContext;
    protected readonly Pilot.Receiver.Data.DataContext AssertReceiverContext;
    protected readonly IServiceScope ReceiverScope;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;
    protected IMapper ReceiverMapper;

    protected readonly IPublishEndpoint PublishEndpoint;

    protected BaseReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory)
    {
        // Порядок важен для правильной инициализации подключений к сервисам
        
        ReceiverScope = receiverFactory.Services.CreateScope();
        ReceiverContext = ReceiverScope.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        AssertReceiverContext = ReceiverScope.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        ReceiverClient = receiverFactory.CreateClient();
        PublishEndpoint = ReceiverScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        IdentityClient = identityFactory.CreateClient();

        ReceiverMapper = ReceiverScope.ServiceProvider.GetRequiredService<IMapper>();
        
        HttpSingleTone.Init.HttpClients["Receiver.IdentityServer"] = IdentityClient;
    }
}