﻿using MassTransit;
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
    protected readonly Pilot.Receiver.Data.DataContext AssertReceiverContext;
    protected readonly IServiceScope ReceiverScope;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;
    
    protected readonly IPublishEndpoint PublishEndpoint;

    protected BaseReceiverIntegrationTest(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory)
    {
        // Порядок важен для правильной инициализации подключений к сервисам
        
        ReceiverScope = receiverFactory.Services.CreateScope();
        ReceiverContext = ReceiverScope.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        AssertReceiverContext = receiverFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        ReceiverClient = receiverFactory.CreateClient();
        PublishEndpoint = ReceiverScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        IdentityClient = identityFactory.CreateClient();

        HttpSingleTone.Init.HttpClients["Receiver.IdentityServer"] = IdentityClient;
    }
}