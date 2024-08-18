using System.Data;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Data;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public class BaseHttpService(
    ILogger<BaseHttpService> logger,
    IHttpClientFactory httpClientFactory)
    : IBaseHttpService
{
    protected readonly ILogger<BaseHttpService> Logger = logger;

    private HttpClient? _httpClient;

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == null)
                throw new NullReferenceException("Null http client");

            return _httpClient;
        }
    }

    public async Task<ICollection<TOut>> SendGetMessages<TOut>(string url, CancellationToken token)
    {
        Logger.LogInformation($"Send message to {url}");

        HttpClientInit<TOut>();

        var response = await HttpClient.GetAsync(url, token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<List<TOut>>(token);
        if (content == null) throw new BadRequestException($"Content from {url} is null");

        return content;
    }

    public async Task<TOut> SendGetMessage<TOut>(string url, CancellationToken token)
    {
        Logger.LogInformation($"Send message to {url}");

        HttpClientInit<TOut>();

        var response = await HttpClient.GetAsync(url, token);
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null) throw new NotFoundException("User is not found");

        return content;
    }

    private void HttpClientInit<TOut>()
    {
        if (_httpClient != null) throw new DataException("HttpClient уже и так инициализирован");

        var clientName = HttpNameService.GetHttpClientName(typeof(TOut));

        // Для тестов. По другому не придумал, как микросервисы дебажить, а Debug в тестах я люблю
        _httpClient = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
            ? HttpSingleTone.Init.HttpClients[$"{clientName}"]
            : httpClientFactory.CreateClient(clientName);
    }

    // Я все операции Post, Update и Delete делаю через Consumer, поэтому это не надо. Но на крайний случай оставлю
    // public async Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token)
    // {
    //     Logger.LogInformation($"Post message to {url}");
    //     Logger.LogClassInfo(message);
    //     var response = await HttpClient.PostAsJsonAsync(url, message, cancellationToken: token);
    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
    //     }
    // }

    // public async Task SendPutMessage<TMessage>(string url, TMessage message, CancellationToken token)
    // {
    //     Logger.LogInformation($"Put message to {url}");
    //     Logger.LogClassInfo(message);
    //     var response = await HttpClient.PutAsJsonAsync(url, message, cancellationToken: token);
    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
    //     }
    // }
    //
    // public async Task SendDeleteMessage<TMessage>(string url, CancellationToken token)
    // {
    //     Logger.LogInformation($"Delete message to {url}");
    //     var response = await HttpClient.DeleteAsync(url, cancellationToken: token);
    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
    //     }
    // }
}