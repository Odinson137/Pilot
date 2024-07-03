using Pilot.Contracts.Base;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Api.Services;

public class HttpIdentityService : BaseHttpService, IHttpIdentityService
{
    public HttpIdentityService(ILogger<HttpIdentityService> logger, IHttpClientFactory httpClientFactory) 
        : base(logger, httpClientFactory, "IdentityServer")
    {
    }

    public async Task<TOut> SendPostMessage<TOut, TMessage>(string url, TMessage message, CancellationToken token)
    {
        Logger.LogInformation($"Post message to {url}");
        Logger.LogClassInfo(message);
        var response = await HttpClient.PostAsJsonAsync(url, message, cancellationToken: token);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
        }

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null)
        {
            throw new BadRequestException("The content is null");
        }

        return content;
    }
}