using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Data;
using Test.Api.WorkerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.WorkerService;

public class BaseWorkerServiceIntegrationTest : 
    IClassFixture<WorkerTestApiFactory>, 
    IClassFixture<WorkerTestIdentityFactory>,
    IClassFixture<WorkerTestWorkerFactory>,
    IClassFixture<WorkerTestStorageFactory>
{
    protected readonly HttpClient ApiClient;
    protected readonly IServiceProvider WorkerScopeService;
    protected readonly IMapper Mapper;
    protected readonly IToken TokenService;

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();

    private readonly Dictionary<ServiceName, IServiceProvider> _serviceScopes = new();
    private readonly Dictionary<ServiceName, Type> _contextTypes = new()
    {
        { ServiceName.AuditHistoryService, typeof(Pilot.AuditHistory.Data.ClickHouseContext) },
        { ServiceName.WorkerServer, typeof(DataContext) },
        { ServiceName.IdentityServer, typeof(Pilot.Identity.Data.DataContext) },
    };

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        var contextType = _contextTypes[serviceName];
        return (DbContext)_serviceScopes[serviceName].CreateScope().ServiceProvider.GetRequiredService(contextType);
    }
    
    protected BaseWorkerServiceIntegrationTest(
        WorkerTestApiFactory apiFactory, 
        WorkerTestIdentityFactory identityFactory, 
        WorkerTestWorkerFactory workerFactory, 
        WorkerTestStorageFactory storageFactory)
    {
        ApiClient = apiFactory.CreateClient();

        var identityScopeService = identityFactory.Services.CreateScope().ServiceProvider;
        WorkerScopeService = workerFactory.Services.CreateScope().ServiceProvider;
        Mapper = workerFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
        TokenService = apiFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IToken>();

        _serviceScopes[ServiceName.WorkerServer] = WorkerScopeService;
        _serviceScopes[ServiceName.IdentityServer] = identityScopeService;
        _serviceScopes[ServiceName.StorageServer] = workerFactory.Services;
        var workerClient = workerFactory.CreateClient();
        
        HttpSingleTone.Init.HttpClients[nameof(ServiceName.IdentityServer)] = identityFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[nameof(ServiceName.StorageServer)] = storageFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[nameof(ServiceName.WorkerServer)] = workerClient;
    }
}