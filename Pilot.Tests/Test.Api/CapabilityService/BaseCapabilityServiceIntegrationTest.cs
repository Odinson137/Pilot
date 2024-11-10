using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Capability.Data;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Test.Api.CapabilityService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.CapabilityService;

public class BaseCapabilityServiceIntegrationTest : 
    IClassFixture<CapabilityTestApiFactory>, 
    IClassFixture<CapabilityTestIdentityFactory>,
    IClassFixture<CapabilityTestCapabilityFactory>,
    IClassFixture<CapabilityTestStorageFactory>
{
    protected readonly HttpClient ApiClient;
    protected readonly IServiceProvider CapabilityScopeService;
    protected readonly IMapper Mapper;
    protected readonly IToken TokenService;
    
    // protected Pilot.Identity.Data.DataContext AssertCapabilityContext 
    //     => _identityScopeService.CreateScope().ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _contexts[serviceName];
    }
    
    protected DataContext AssertContext 
        => CapabilityScopeService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

    protected BaseCapabilityServiceIntegrationTest(CapabilityTestApiFactory apiFactory, CapabilityTestIdentityFactory identityFactory, CapabilityTestCapabilityFactory capabilityFactory, CapabilityTestStorageFactory storageFactory)
    {
        ApiClient = apiFactory.CreateClient();

        var identityScopeService = identityFactory.Services.CreateScope().ServiceProvider;
        CapabilityScopeService = capabilityFactory.Services.CreateScope().ServiceProvider;
        Mapper = capabilityFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
        TokenService = apiFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IToken>();

        _contexts[ServiceName.IdentityServer] = identityScopeService.GetRequiredService<Pilot.Identity.Data.DataContext>();
        _contexts[ServiceName.StorageServer] = storageFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Storage.Data.DataContext>();
        
        _contexts[ServiceName.CapabilityServer] = CapabilityScopeService.GetRequiredService<DataContext>();
        var capabilityClient = capabilityFactory.CreateClient();
        
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.StorageServer.ToString()] = storageFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.CapabilityServer.ToString()] = capabilityClient;
    }
}