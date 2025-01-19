using Microsoft.Extensions.Logging;
using Pilot.Api.Services;
using Pilot.Contracts.Services;

namespace Test.Base.IntegrationBase;

public class HttpIdentityServiceFaker : HttpIdentityService
{
    protected override void HttpClientInit<TOut>()
    {
        var clientName = HttpNameService.GetHttpClientName(typeof(TOut));
        if (ClientName == clientName) return;
        
        ClientName = clientName;
        // Для тестов. По другому не придумал, как микросервисы дебажить, а Debug в тестах я люблю
        HttpClient = HttpSingleTone.Init.HttpClients[clientName];
    }

    public HttpIdentityServiceFaker(ILogger<HttpIdentityService> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
    {
    }
}