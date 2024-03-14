using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using MongoDB.Driver;
using Xunit;

namespace Pilot.Tests.Identity.Tests;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    public readonly ISender Sender;
    public  readonly HttpClient Client;
    public  readonly IMongoDatabase MongoDatabase;
    public  readonly IServiceScope ScopeService;
    
    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        ScopeService = factory.Services.CreateScope();

        MongoDatabase = ScopeService.ServiceProvider.GetRequiredService<IMongoDatabase>();

        Client = factory.CreateClient();
    }
}