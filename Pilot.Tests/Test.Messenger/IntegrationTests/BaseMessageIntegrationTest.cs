using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Messenger.Data;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class BaseMessageIntegrationTest : IClassFixture<MessageTestMessageFactory>, IClassFixture<MessageTestIdentityFactory>
{
    protected readonly DataContext DataContext;
    protected readonly Pilot.Identity.Data.DataContext IdentityDataContext;
    protected readonly HttpClient MessengerClient;

    private readonly IServiceProvider _messageService;
    protected DataContext AssertContext 
        => _messageService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    
    protected readonly IPublishEndpoint PublishEndpoint;

    public IMapper MessengerMapper { get; set; }

    protected BaseMessageIntegrationTest(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    {
        _messageService = factory.Services;
        var messengerScope = factory.Services.CreateScope();
        DataContext = messengerScope.ServiceProvider.GetRequiredService<DataContext>();
        IdentityDataContext = identityFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();

        PublishEndpoint = messengerScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        MessengerClient = factory.CreateClient();

        MessengerMapper = messengerScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}