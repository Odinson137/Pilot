using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.BackgroundJob.Data;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Test.Api.BackgroundJobService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.BackgroundJobService;

public class BaseBackgroundJobServiceIntegrationTest : 
    IClassFixture<BackgroundJobTestApiFactory>, 
    IClassFixture<BackgroundJobTestIdentityFactory>,
    IClassFixture<BackgroundJobTestBackgroundJobFactory>,
    IClassFixture<BackgroundJobTestStorageFactory>
{
    protected readonly HttpClient ApiClient;
    protected readonly IServiceProvider ScopeService;
    protected readonly IMapper Mapper;
    protected readonly IToken TokenService;

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _contexts[serviceName];
    }
    
    protected DataContext AssertContext 
        => ScopeService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

    protected BaseBackgroundJobServiceIntegrationTest(BackgroundJobTestApiFactory apiFactory, BackgroundJobTestIdentityFactory identityFactory, BackgroundJobTestBackgroundJobFactory capabilityFactory, BackgroundJobTestStorageFactory storageFactory)
    {
        ApiClient = apiFactory.CreateClient();

        var identityScopeService = identityFactory.Services.CreateScope().ServiceProvider;
        ScopeService = capabilityFactory.Services.CreateScope().ServiceProvider;
        Mapper = capabilityFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
        TokenService = apiFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IToken>();

        _contexts[ServiceName.IdentityServer] = identityScopeService.GetRequiredService<Pilot.Identity.Data.DataContext>();
        _contexts[ServiceName.StorageServer] = storageFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Storage.Data.DataContext>();
        
        _contexts[ServiceName.BackgroundJobService] = ScopeService.GetRequiredService<DataContext>();
        var capabilityClient = capabilityFactory.CreateClient();
        
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.StorageServer.ToString()] = storageFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.BackgroundJobService.ToString()] = capabilityClient;
    }
}