using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Identity.Data;
using Test.Api.IntegrationTests.Factories;
using Test.Base.IntegrationBase;

namespace Test.Api.IntegrationTests;

public class BaseApiIntegrationTest : IClassFixture<ApiTestApiFactory>, IClassFixture<ApiTestWorkerFactory>,
    IClassFixture<ApiTestIdentityFactory>
{
    private readonly IServiceProvider _receiverScopeService;
    protected readonly IToken TokenService;
    protected readonly HttpClient ApiClient;
    protected readonly IMapper ReceiverMapper;
    protected Pilot.Worker.Data.DataContext AssertReceiverContext 
        => _receiverScopeService.CreateScope().ServiceProvider.GetRequiredService<Pilot.Worker.Data.DataContext>();

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _contexts[serviceName];
    }
    
    protected BaseApiIntegrationTest(ApiTestApiFactory apiFactory, ApiTestWorkerFactory workerFactory,
        ApiTestIdentityFactory identityFactory)
    {
        _receiverScopeService = workerFactory.Services;
        var receiverScopeService = _receiverScopeService.CreateScope();
        _contexts[ServiceName.WorkerServer] = receiverScopeService.ServiceProvider.GetRequiredService<Pilot.Worker.Data.DataContext>();
        
        ReceiverMapper = receiverScopeService.ServiceProvider.GetRequiredService<IMapper>();

        var identityScopeService = identityFactory.Services.CreateScope();
        _contexts[ServiceName.IdentityServer] = identityScopeService.ServiceProvider.GetRequiredService<DataContext>();
        
        // _contexts[ServiceName.CapabilityServer] = capabilityFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Capability.Data.DataContext>();

        TokenService = apiFactory.Services.GetRequiredService<IToken>();
        
        var receiverClient = workerFactory.CreateClient();
        var identityClient = identityFactory.CreateClient();
        
        HttpSingleTone.Init.HttpClients[ServiceName.WorkerServer.ToString()] = receiverClient;
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityClient;
        HttpSingleTone.Init.HttpClients[ServiceName.CapabilityServer.ToString()] = identityClient;

        ApiClient = apiFactory.CreateClient();
    }
}