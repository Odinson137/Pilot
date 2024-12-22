using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Pilot.BackgroundJob.Data;
using Test.BackgroundJob.Factories;

namespace Test.BackgroundJob.IntegrationTests;

public class BaseBackgroundJobIntegrationTest : IClassFixture<BackgroundJobTestBackgroundJobFactory>, IClassFixture<BackgroundJobTestIdentityFactory>
{
    protected readonly DataContext DataContext;
    protected readonly HttpClient CapabilityClient;
    protected readonly Pilot.Identity.Data.DataContext IdentityDataContext;

    protected readonly IServiceScope BackgroundJobScope;
    protected DataContext AssertContext 
        => BackgroundJobScope.ServiceProvider.GetRequiredService<DataContext>();
    
    protected IMapper ReceiverMapper;
    protected readonly IPublishEndpoint PublishEndpoint;
    
    protected BaseBackgroundJobIntegrationTest(BackgroundJobTestBackgroundJobFactory factory, BackgroundJobTestIdentityFactory identityFactory)
    {
        BackgroundJobScope = factory.Services.CreateScope();
        DataContext = BackgroundJobScope.ServiceProvider.GetRequiredService<DataContext>();
        IdentityDataContext = identityFactory.Services.CreateScope().ServiceProvider.GetRequiredService<Pilot.Identity.Data.DataContext>();

        CapabilityClient = factory.CreateClient();
        PublishEndpoint = BackgroundJobScope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        ReceiverMapper = BackgroundJobScope.ServiceProvider.GetRequiredService<IMapper>();
    }
}