﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Capability.Data;
using Test.Capability.Factories;

namespace Test.Capability.IntegrationTests;

public class BaseCapabilityIntegrationTest : IClassFixture<CapabilityTestCapabilityFactory>
{
    protected readonly DataContext DataContext;
    protected readonly HttpClient CapabilityClient;

    protected readonly IServiceScope CapabilityScope;
    protected IMapper ReceiverMapper;

    protected BaseCapabilityIntegrationTest(CapabilityTestCapabilityFactory factory)
    {
        CapabilityScope = factory.Services.CreateScope();
        DataContext = CapabilityScope.ServiceProvider.GetRequiredService<DataContext>();
        var assertReceiverContext = CapabilityScope.ServiceProvider.GetRequiredService<DataContext>();

        CapabilityClient = factory.CreateClient();

        ReceiverMapper = CapabilityScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}