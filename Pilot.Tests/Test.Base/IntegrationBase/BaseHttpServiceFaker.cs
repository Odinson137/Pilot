using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Services;

namespace Test.Base.IntegrationBase;

public class BaseHttpServiceFaker(ILogger<BaseHttpServiceFaker> logger, IHttpClientFactory httpClientFactory) : BaseHttpService(logger, httpClientFactory)
{
    protected override void HttpClientInit<TOut>()
    {
        var clientName = HttpNameService.GetHttpClientName(typeof(TOut));
        
        // Для тестов. По другому не придумал, как микросервисы дебажить, а Debug в тестах я люблю
        HttpClient = HttpSingleTone.Init.HttpClients[clientName];
    }
}