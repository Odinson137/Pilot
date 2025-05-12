using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Test.Base.IntegrationBase;

public abstract class BaseIntegrationTest : IDisposable
{
    protected readonly HttpClient ApiClient;
    protected readonly IMapper Mapper;
    protected readonly IToken TokenService;

    private readonly Dictionary<ServiceName, IServiceProvider> _serviceScopes = new();
    private readonly Dictionary<ServiceName, Type> _contextTypes = new();
    private readonly Dictionary<ServiceName, HttpClient> _httpClients = new();

    protected DbContext GetContext(ServiceName serviceName)
    {
        var contextType = _contextTypes[serviceName];
        return (DbContext)_serviceScopes[serviceName].CreateScope().ServiceProvider.GetRequiredService(contextType);
    }

    protected BaseIntegrationTest(ServiceTestConfiguration? apiConfiguration = null,
        IEnumerable<ServiceTestConfiguration>? configurations = null)
    {
        if (apiConfiguration != null)
        {
            ApiClient = apiConfiguration.HttpClient;
            TokenService = apiConfiguration.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IToken>();
        }

        if (configurations == null) return;

        foreach (var config in configurations)
        {
            _serviceScopes[config.ServiceName] = config.ServiceProvider;
            _httpClients[config.ServiceName] = config.HttpClient;
            _contextTypes[config.ServiceName] = config.DbContextType;

            // Регистрация HTTP-клиента в HttpSingleTone
            HttpSingleTone.Init.HttpClients[config.ServiceName.ToString()] = config.HttpClient;

            if (config.IsMainService)
                Mapper = config.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
        }
    }

    public void Dispose()
    {
        foreach (var client in _httpClients.Values)
        {
            client.Dispose();
        }
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