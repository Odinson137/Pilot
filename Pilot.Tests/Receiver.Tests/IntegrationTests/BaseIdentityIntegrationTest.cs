using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pilot.Identity.Data;
using Pilot.Identity.Services;
using Xunit;

namespace Pilot.Tests.Receiver.Tests.IntegrationTests;

public class BaseReceiverIntegrationTest : IClassFixture<IntegrationReceiverTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IMongoDatabase MongoDatabase;
    protected readonly IServiceScope ScopeService;
    
    public BaseReceiverIntegrationTest(IntegrationReceiverTestWebAppFactory factory)
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
