using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pilot.Identity.Data;
using Pilot.Identity.Services;
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

    protected void Authorize()
    {
        var token = new TokenService().GenerateToken("admin", Role.Admin);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected void UnAuthorize()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }
}
