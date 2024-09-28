using Microsoft.Extensions.DependencyInjection;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;
using Test.Identity.IntegrationTests.Factories;

namespace Test.Identity.IntegrationTests;

public class BaseIdentityIntegrationTest : IClassFixture<IntegrationIdentityTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IServiceScope ScopeService;
    protected readonly DataContext DataContext;
    protected readonly IPasswordCoder CoderService;
    
    protected DataContext AssertContext 
        => ScopeService.ServiceProvider.GetRequiredService<DataContext>();
    
    protected BaseIdentityIntegrationTest(IntegrationIdentityTestWebAppFactory factory)
    {
        ScopeService = factory.Services.CreateScope();
        DataContext = ScopeService.ServiceProvider.GetRequiredService<DataContext>();
        CoderService = ScopeService.ServiceProvider.GetRequiredService<IPasswordCoder>();

        Client = factory.CreateClient();
    }
}