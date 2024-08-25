using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Api.Services;
using Pilot.Contracts.Data;
using Pilot.Identity.Data;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class BaseApiIntegrationTest : IClassFixture<ApiTestApiFactory>, IClassFixture<ApiTestReceiverFactory>,
    IClassFixture<ApiTestIdentityFactory>
{
    private readonly IServiceProvider _receiverScopeService;
    protected readonly IToken TokenService;
    protected readonly HttpClient ApiClient;
    protected readonly HttpClient IdentityClient;
    protected readonly DataContext IdentityContext;
    protected readonly HttpClient ReceiverClient;
    protected readonly IMapper ReceiverMapper;
    protected Pilot.Receiver.Data.DataContext AssertReceiverContext 
        => _receiverScopeService.CreateScope().ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
    protected readonly Pilot.Receiver.Data.DataContext ReceiverContext;

    protected BaseApiIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
    {
        _receiverScopeService = receiverFactory.Services;
        var receiverScopeService = _receiverScopeService.CreateScope();
        ReceiverContext = receiverScopeService.ServiceProvider.GetRequiredService<Pilot.Receiver.Data.DataContext>();
        ReceiverMapper = receiverScopeService.ServiceProvider.GetRequiredService<IMapper>();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<DataContext>();

        TokenService = apiFactory.Services.GetRequiredService<IToken>();
        
        ReceiverClient = receiverFactory.CreateClient();
        IdentityClient = identityFactory.CreateClient();
        HttpSingleTone.Init.HttpClients["ReceiverServer"] = ReceiverClient;
        HttpSingleTone.Init.HttpClients["IdentityServer"] = IdentityClient;

        ApiClient = apiFactory.CreateClient();
    }
}