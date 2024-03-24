using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pilot.Tests.Identity.Tests;
using Xunit;

namespace Pilot.Tests.Api.Tests.IntegrationTests;

public class BaseApiIntegrationTest : IClassFixture<IntegrationApiTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IMongoDatabase MongoDatabase;
    protected readonly IServiceScope ScopeService;
    
    public BaseApiIntegrationTest(IntegrationApiTestWebAppFactory factory)
    {
        ScopeService = factory.Services.CreateScope();

        MongoDatabase = ScopeService.ServiceProvider.GetRequiredService<IMongoDatabase>();

        Client = factory.CreateClient();
    }
}