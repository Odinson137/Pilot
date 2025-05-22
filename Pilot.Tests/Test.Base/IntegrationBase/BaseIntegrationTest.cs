using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Data.Enums;

namespace Test.Base.IntegrationBase;

public abstract class BaseIntegrationTest : IDisposable
{
    protected readonly HttpClient Client;
    protected readonly IMapper Mapper;
    protected readonly IToken? TokenService;

    private readonly Dictionary<ServiceName, IServiceProvider> _serviceScopes = new();
    private readonly Dictionary<ServiceName, Type> _contextTypes = new();
    private readonly Dictionary<ServiceName, HttpClient> _httpClients = new();
    protected readonly IPublishEndpoint Publisher;

    protected DbContext GetContext(ServiceName serviceName)
    {
        var contextType = _contextTypes[serviceName];
        return (DbContext)_serviceScopes[serviceName].CreateScope().ServiceProvider.GetRequiredService(contextType);
    }

    protected bool ExistServiceContext(params ServiceName[] serviceNames) =>
        serviceNames.Any(c => _contextTypes.TryGetValue(c, out _));

    protected BaseIntegrationTest(ServiceTestConfiguration? apiConfiguration = null,
        IEnumerable<ServiceTestConfiguration>? configurations = null)
    {
        if (apiConfiguration != null)
        {
            Client = apiConfiguration.HttpClient;
            TokenService = apiConfiguration.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IToken>();
        }

        if (configurations == null) return;

        var testConfigurations = configurations as ServiceTestConfiguration[] ?? configurations.ToArray();
        if (testConfigurations.All(c => c.IsMainService == false)) throw new Exception("Нет выбран главный сервис");

        var serviceTestConfigurations = testConfigurations.ToList();
        foreach (var config in serviceTestConfigurations)
        {
            _serviceScopes[config.ServiceName] = config.ServiceProvider;
            _httpClients[config.ServiceName] = config.HttpClient;
            _contextTypes[config.ServiceName] = config.DbContextType;

            // Регистрация HTTP-клиента в HttpSingleTone
            HttpSingleTone.Init.HttpClients[config.ServiceName.ToString()] = config.HttpClient;

            if (config.IsMainService)
            {
                Mapper = config.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
                if (apiConfiguration == null)
                    Client = config.HttpClient;
            }
        }

        Publisher = serviceTestConfigurations.First().ServiceProvider.CreateScope().ServiceProvider
            .GetRequiredService<IPublishEndpoint>();
    }

    public void Dispose()
    {
        foreach (var client in _httpClients.Values)
        {
            client.Dispose();
        }

        foreach (var serviceName in _contextTypes.Keys)
        {
            var contextType = _contextTypes[serviceName];
            var dbContext = (DbContext)_serviceScopes[serviceName].CreateScope().ServiceProvider
                .GetRequiredService(contextType);
            
            if (serviceName == ServiceName.AuditHistoryService) continue;
            
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        HttpSingleTone.Dispose();
    }
}

public class ServiceTestConfiguration
{
    public required ServiceName ServiceName { get; set; }

    public required IServiceProvider ServiceProvider { get; set; }
    public HttpClient HttpClient { get; set; }

    // public WebApplicationFactory<T> Factory { get; set; }

    public bool IsMainService { get; set; } = false; // тот, который тестирую
    public Type DbContextType { get; set; }
}