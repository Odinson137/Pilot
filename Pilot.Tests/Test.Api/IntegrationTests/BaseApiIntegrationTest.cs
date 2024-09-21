using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
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
    protected readonly DataContext IdentityContext;
    protected readonly IMapper ReceiverMapper;
    protected Pilot.Worker.Data.DataContext AssertReceiverContext 
        => _receiverScopeService.CreateScope().ServiceProvider.GetRequiredService<Pilot.Worker.Data.DataContext>();
    protected readonly Pilot.Worker.Data.DataContext ReceiverContext;

    protected BaseApiIntegrationTest(ApiTestApiFactory apiFactory, ApiTestReceiverFactory receiverFactory,
        ApiTestIdentityFactory identityFactory)
    {
        _receiverScopeService = receiverFactory.Services;
        var receiverScopeService = _receiverScopeService.CreateScope();
        ReceiverContext = receiverScopeService.ServiceProvider.GetRequiredService<Pilot.Worker.Data.DataContext>();
        ReceiverMapper = receiverScopeService.ServiceProvider.GetRequiredService<IMapper>();

        var identityScopeService = identityFactory.Services.CreateScope();
        IdentityContext = identityScopeService.ServiceProvider.GetRequiredService<DataContext>();

        TokenService = apiFactory.Services.GetRequiredService<IToken>();
        
        var receiverClient = receiverFactory.CreateClient();
        var identityClient = identityFactory.CreateClient();
        
        HttpSingleTone.Init.HttpClients["ReceiverServer"] = receiverClient;
        HttpSingleTone.Init.HttpClients["IdentityServer"] = identityClient;

        ApiClient = apiFactory.CreateClient();
    }
}