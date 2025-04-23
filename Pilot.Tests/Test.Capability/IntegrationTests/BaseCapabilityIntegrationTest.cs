using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Capability.Data;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class BaseCapabilityIntegrationTest : IClassFixture<CapabilityTestCapabilityFactory>
{
    protected readonly IPublishEndpoint PublishEndpoint;

    protected readonly DataContext DataContext;
    protected readonly IServiceProvider CapabilityService;
    protected readonly HttpClient CapabilityClient;
    protected readonly Pilot.Identity.Data.DataContext IdentityContext;

    protected readonly IServiceScope CapabilityScope;
    protected DataContext AssertContext 
        => CapabilityScope.ServiceProvider.GetRequiredService<DataContext>();
    
    protected IMapper ReceiverMapper;
    
    protected DataContext AssertReceiverContext
        => CapabilityService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseCapabilityIntegrationTest(CapabilityTestCapabilityFactory factory)
    {
        CapabilityService = factory.Services;
        CapabilityScope = CapabilityService.CreateScope();
        PublishEndpoint = CapabilityScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        DataContext = CapabilityScope.ServiceProvider.GetRequiredService<DataContext>();

        CapabilityClient = factory.CreateClient();

        ReceiverMapper = CapabilityScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}