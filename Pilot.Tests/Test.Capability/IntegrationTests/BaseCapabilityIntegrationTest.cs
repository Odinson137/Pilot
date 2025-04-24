using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Capability.Data;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class BaseCapabilityIntegrationTest : IClassFixture<CapabilityTestCapabilityFactory>
{
    protected readonly IPublishEndpoint PublishEndpoint;

    protected readonly IServiceProvider CapabilityService;
    protected readonly HttpClient CapabilityClient;

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

        CapabilityClient = factory.CreateClient();

        ReceiverMapper = CapabilityScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}