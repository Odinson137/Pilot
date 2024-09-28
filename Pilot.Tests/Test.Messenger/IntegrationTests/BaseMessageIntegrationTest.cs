using Microsoft.Extensions.DependencyInjection;
using Pilot.Messenger.Data;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class BaseMessageIntegrationTest : IClassFixture<MessageTestCapabilityFactory>
{
    protected readonly DataContext DataContext;
    protected readonly HttpClient MessengerClient;

    private readonly IServiceScope _messengerScope;
    protected DataContext AssertContext 
        => _messengerScope.ServiceProvider.GetRequiredService<DataContext>();
    
    // protected IMapper MessengerMapper;

    protected BaseMessageIntegrationTest(MessageTestCapabilityFactory factory)
    {
        _messengerScope = factory.Services.CreateScope();
        DataContext = _messengerScope.ServiceProvider.GetRequiredService<DataContext>();

        MessengerClient = factory.CreateClient();

        // MessengerMapper = MessengerScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}