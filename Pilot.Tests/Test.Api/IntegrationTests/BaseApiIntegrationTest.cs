using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Test.Api.IntegrationTests.Factories;
using Xunit;

namespace Test.Api.IntegrationTests;

public class BaseApiIntegrationTest : IClassFixture<ApiTestApiFactory>, IClassFixture<ApiTestReceiverFactory>, IClassFixture<ApiTestIdentityFactory>
{
    protected readonly HttpClient ApiClient;
    protected readonly HttpClient ReceiverClient;
    protected readonly HttpClient IdentityClient;
    protected readonly Pilot.Receiver.Data.DataContext ReceiverContext;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;

    protected BaseApiIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory, ApiTestIdentityFactory identityFactory)
    {
        // Порядок важен для правильной инициализации подключений к сервисам
        
        var receiverScopeService = receiverFactory.Services.CreateScope();
        ReceiverContext = receiverScopeService.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        ReceiverClient = receiverFactory.CreateClient();
        
        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();
        IdentityClient = identityFactory.CreateClient();

        HttpSingleTone.Init.HttpClients["Api.ReceiverServer"] = ReceiverClient;
        HttpSingleTone.Init.HttpClients["Api.IdentityServer"] = IdentityClient;
        
        ApiClient = apiFactory.CreateClient();
    }
}