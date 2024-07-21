using Pilot.Contracts.Base;

namespace Pilot.Api.Services;

public class HttpReceiverService : BaseHttpService, IHttpReceiverService
{
    public HttpReceiverService(ILogger<HttpReceiverService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        : base(logger, httpClientFactory, configuration,"ReceiverServer")
    {
    }
}