using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Identity.Interfaces;
using Test.Api.WorkerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.WorkerService;

public class BaseWorkerServiceIntegrationTest : 
    IClassFixture<WorkerTestApiFactory>, 
    IClassFixture<WorkerTestIdentityFactory>,
    IClassFixture<WorkerTestWorkerFactory>,
    IClassFixture<WorkerTestStorageFactory>
{
    private readonly IServiceProvider _workerScopeService;
    protected readonly IToken TokenService;
    protected readonly IPasswordCoder PasswordCoder;
    protected readonly HttpClient ApiClient;
    
    // protected Pilot.Identity.Data.DataContext AssertWorkerContext 
    //     => _identityScopeService.CreateScope().ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();

    private readonly Dictionary<ServiceName, DbContext> _contexts = new();

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _contexts[serviceName];
    }

    private HttpClient _workerClient;
    protected BaseWorkerServiceIntegrationTest(WorkerTestApiFactory apiFactory, WorkerTestIdentityFactory identityFactory, WorkerTestWorkerFactory workerFactory, WorkerTestStorageFactory storageFactory)
    {
        ApiClient = apiFactory.CreateClient();

        var identityScopeService = identityFactory.Services.CreateScope().ServiceProvider;
        _workerScopeService = workerFactory.Services.CreateScope().ServiceProvider;
        
        _contexts[ServiceName.IdentityServer] = identityScopeService.GetRequiredService<Pilot.Identity.Data.DataContext>();
        _contexts[ServiceName.StorageServer] = storageFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Storage.Data.DataContext>();
        _contexts[ServiceName.WorkerServer] = _workerScopeService.GetRequiredService<Pilot.Worker.Data.DataContext>();
        _workerClient = workerFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.StorageServer.ToString()] = storageFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.WorkerServer.ToString()] =_workerClient ;

        TokenService = apiFactory.Services.GetRequiredService<IToken>();
        PasswordCoder = identityScopeService.GetRequiredService<IPasswordCoder>();
    }
}