using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Pilot.Tests.Receiver.Tests.IntegrationTests.Factories;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class BaseReceiverIntegrationTest : IClassFixture<ReceiverTestReceiverFactory>, IClassFixture<ReceiverTestIdentityFactory>
{
    protected readonly HttpClient ReceiverClient;
    protected readonly HttpClient IdentityClient;
    protected readonly Pilot.Receiver.Data.DataContext ReceiverContext;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;
    
    protected readonly IPublishEndpoint PublishEndpoint;

    protected BaseReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory)
    {
        // Порядок важен для правильной инициализации подключений к сервисам
        
        var receiverScopeService = receiverFactory.Services.CreateScope();
        ReceiverContext = receiverScopeService.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        ReceiverClient = receiverFactory.CreateClient();
        PublishEndpoint = receiverScopeService.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        IdentityClient = identityFactory.CreateClient();

        HttpSingleTone.Init.HttpClients["IdentityServer"] = IdentityClient;
    }
}