using Microsoft.Extensions.Logging;
using Pilot.Contracts.Base;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;

namespace Test.Base.IntegrationBase.Fakers;

public class ModelServiceFaker : ModelService
{
    protected override void HttpClientInit<TOut>()
    {
        var clientName = HttpNameService.GetHttpClientName(typeof(TOut));
        if (ClientName == clientName) return;
        
        ClientName = clientName;
        // Для тестов. По другому не придумал, как микросервисы дебажить, а Debug в тестах я люблю
        HttpClient = HttpSingleTone.Init.HttpClients[clientName];

    }

    public ModelServiceFaker(ILogger<ModelService> logger, IHttpClientFactory httpClientFactory, IRedisService redis) : base(logger, httpClientFactory, redis)
    {
    }
}