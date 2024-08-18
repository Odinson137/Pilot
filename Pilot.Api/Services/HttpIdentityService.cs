using Pilot.Api.Interfaces;
using Pilot.Contracts.Base;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Api.Services;

public class HttpIdentityService(
    ILogger<HttpIdentityService> logger,
    IHttpClientFactory httpClientFactory)
    : BaseHttpService(logger, httpClientFactory), IHttpIdentityService
{
    public async Task<TOut> SendPostMessage<TOut, TMessage>(string url, TMessage message, CancellationToken token)
    {
        Logger.LogInformation($"Post message to {url}");
        Logger.LogClassInfo(message);
        var response = await HttpClient.PostAsJsonAsync(url, message, token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null) throw new BadRequestException("The content is null");

        return content;
    }

    public async Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token)
    {
        Logger.LogInformation($"Post message to {url}");
        Logger.LogClassInfo(message);
        var response = await HttpClient.PostAsJsonAsync(url, message, token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));
    }
}