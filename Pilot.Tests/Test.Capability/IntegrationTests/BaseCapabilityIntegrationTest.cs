using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Capability.Data;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class BaseCapabilityIntegrationTest : IClassFixture<CapabilityTestCapabilityFactory>
{
    protected readonly IPublishEndpoint PublishEndpoint;

    private readonly IServiceProvider _capabilityService;
    protected readonly HttpClient CapabilityClient;

    protected readonly IMapper ReceiverMapper;
    
    protected DataContext AssertReceiverContext
        => _capabilityService.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseCapabilityIntegrationTest(CapabilityTestCapabilityFactory factory)
    {
        _capabilityService = factory.Services;
        var capabilityScope = _capabilityService.CreateScope();
        PublishEndpoint = capabilityScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        CapabilityClient = factory.CreateClient();

        ReceiverMapper = capabilityScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}