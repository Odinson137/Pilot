using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Services.LogService;

namespace Pilot.Contracts.Base;

public class BaseHttpService : IBaseHttpService
{
    protected readonly ILogger<BaseHttpService> Logger;
    protected readonly HttpClient HttpClient;

    protected BaseHttpService(
        ILogger<BaseHttpService> logger, 
        IHttpClientFactory httpClientFactory,
        string clientName)
    {
        Logger = logger;
        HttpClient = httpClientFactory.CreateClient(clientName);
    }
    
    public async Task<TOut> SendGetMessage<TOut>(string url, BaseFilter? filter, CancellationToken token)
    {
        Logger.LogInformation($"Send message to {url}");
        var response = await HttpClient.GetAsync(url, token);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
        }

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null)
        {
            throw new BadRequestException($"Content from {url} is null");
        }
        
        return content;
    }

    public async Task<TOut?> SendGetOneMessage<TOut>(string url, CancellationToken token)
    {
        Logger.LogInformation($"Send message to {url}");
        var response = await HttpClient.GetAsync(url, token);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
        }

        var content = await response.Content.ReadFromJsonAsync<TOut>(token);
        if (content == null)
        {
            throw new NotFoundException("User is not found");
        }
        
        return content;
    }
    
    public async Task SendPostMessage<TMessage>(string url, TMessage message, CancellationToken token)
    {
        Logger.LogInformation($"Post message to {url}");
        Logger.LogClassInfo(message);
        var response = await HttpClient.PostAsJsonAsync(url, message, cancellationToken: token);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
        }
    }

    public async Task SendPutMessage<TMessage>(string url, TMessage message, CancellationToken token)
    {
        Logger.LogInformation($"Put message to {url}");
        Logger.LogClassInfo(message);
        var response = await HttpClient.PutAsJsonAsync(url, message, cancellationToken: token);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
        }
    }

    public async Task SendDeleteMessage<TMessage>(string url, CancellationToken token)
    {
        Logger.LogInformation($"Delete message to {url}");
        var response = await HttpClient.DeleteAsync(url, cancellationToken: token);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(token));   
        }
    }
}