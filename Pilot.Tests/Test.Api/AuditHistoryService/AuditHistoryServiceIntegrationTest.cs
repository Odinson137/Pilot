using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Test.Api.AuditHistoryService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.AuditHistoryService;

public class BaseAuditHistoryIntegrationTest : 
    IClassFixture<AuditHistoryTestApiFactory>, 
    IClassFixture<AuditHistoryTestAuditHistoryFactory>,
    IClassFixture<AuditHistoryTestWorkerFactory>,
    IClassFixture<AuditHistoryTestIdentityFactory>
{
    protected readonly HttpClient ApiClient;
    protected readonly IToken TokenService;

    // Наверное это лучшая реализация получения контекстов в тесты.
    // TODO Позже реализовать такую логику в других тестах 
    private readonly Dictionary<ServiceName, IServiceProvider> _serviceScopes = new();
    private readonly Dictionary<ServiceName, Type> _contextTypes = new()
    {
        { ServiceName.AuditHistoryService, typeof(Pilot.AuditHistory.Data.ClickHouseContext) },
        { ServiceName.WorkerServer, typeof(Pilot.Worker.Data.DataContext) },
        { ServiceName.IdentityServer, typeof(Pilot.Identity.Data.DataContext) },
    };

    protected DbContext GetContext<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        var contextType = _contextTypes[serviceName];
        return (DbContext)_serviceScopes[serviceName].CreateScope().ServiceProvider.GetRequiredService(contextType);
    }

    protected IMapper GetMapper<TDto>() where TDto : BaseDto
    {
        var serviceName = typeof(TDto).GetCustomAttribute<FromServiceAttribute>()?.ServiceName ??
                          throw new Exception("У dto нет атрибута FromServiceAttribute");
        
        return _serviceScopes[serviceName].CreateScope().ServiceProvider.GetRequiredService<IMapper>();
    }

    protected BaseAuditHistoryIntegrationTest(
        AuditHistoryTestApiFactory apiFactory, 
        AuditHistoryTestAuditHistoryFactory auditHistoryFactory,
        AuditHistoryTestWorkerFactory workerFactory,
        AuditHistoryTestIdentityFactory identityFactory
        )
    {
        ApiClient = apiFactory.CreateClient();

        TokenService = apiFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IToken>();

        _serviceScopes[ServiceName.AuditHistoryService] = auditHistoryFactory.Services;
        _serviceScopes[ServiceName.WorkerServer] = workerFactory.Services;
        _serviceScopes[ServiceName.IdentityServer] = identityFactory.Services;
        
        HttpSingleTone.Init.HttpClients[ServiceName.AuditHistoryService.ToString()] = auditHistoryFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.WorkerServer.ToString()] = workerFactory.CreateClient();
        HttpSingleTone.Init.HttpClients[ServiceName.IdentityServer.ToString()] = identityFactory.CreateClient();
    }
}