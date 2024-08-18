using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Data;
using Pilot.Identity.Data;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class BaseApiIntegrationTest : IClassFixture<ApiTestApiFactory>, IClassFixture<ApiTestReceiverFactory>,
    IClassFixture<ApiTestIdentityFactory>
{
    protected readonly HttpClient ApiClient;
    protected readonly HttpClient IdentityClient;
    protected readonly DataContext IdentityContext;
    protected readonly HttpClient ReceiverClient;
    protected readonly Pilot.Receiver.Data.DataContext ReceiverContext;

    protected BaseApiIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
    {
        // Порядок важен для правильной инициализации подключений к сервисам

        var receiverScopeService = receiverFactory.Services.CreateScope();
        ReceiverContext = receiverScopeService.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        ReceiverClient = receiverFactory.CreateClient();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<DataContext>();
        IdentityClient = identityFactory.CreateClient();

        HttpSingleTone.Init.HttpClients["ReceiverServer"] = ReceiverClient;
        HttpSingleTone.Init.HttpClients["IdentityServer"] = IdentityClient;

        ApiClient = apiFactory.CreateClient();
    }
}