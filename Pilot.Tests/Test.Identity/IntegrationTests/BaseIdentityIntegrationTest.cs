using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Test.Identity.IntegrationTests.Factories;

namespace Test.Identity.IntegrationTests;

public class BaseIdentityIntegrationTest : IClassFixture<IntegrationIdentityTestWebAppFactory>
{
    protected readonly HttpClient Client;
    private readonly IServiceProvider _serviceProvider;
    protected readonly DataContext DataContext;
    protected readonly IPasswordCoder CoderService;
    protected readonly IMapper Mapper;
    protected readonly IPublishEndpoint PublishEndpoint;

    protected DataContext AssertReceiverContext 
        => _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseIdentityIntegrationTest(IntegrationIdentityTestWebAppFactory factory)
    {
        _serviceProvider = factory.Services;
        var scopeService = factory.Services.CreateScope();
        DataContext = scopeService.ServiceProvider.GetRequiredService<DataContext>();
        Mapper = scopeService.ServiceProvider.GetRequiredService<IMapper>();
        PublishEndpoint = scopeService.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        Client = factory.CreateClient();
    }
}