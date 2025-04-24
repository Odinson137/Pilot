using System.Net.Http.Headers;
using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service;

public class GateWayApiService(
    ILogger<BaseHttpService> logger, 
    IHttpClientFactory httpClientFactory, 
    IMapper mapper,
    TokenAuthenticationStateProvider tokenAuthenticationStateProvider)
    : BaseHttpService(logger, httpClientFactory), IGateWayApiService
{
    public bool Authorizate { get; set; } = true;
    
    public async Task<ICollection<TViewModel>> SendGetMessages<TOut, TViewModel>(
        string? url = null,
        BaseFilter filter = null!,
        CancellationToken token = default)
        where TOut : BaseDto where TViewModel : BaseViewModel
    {
        var models = await SendGetMessages<TOut>(url, filter, token);
        return models.MapList<TViewModel>(mapper);
    }

    public async Task<TViewModel> SendGetMessage<TOut, TViewModel>(
        int valueId,
        CancellationToken token = default)
        where TOut : BaseDto where TViewModel : BaseViewModel
    {
        var model= await SendGetMessage<TOut>($"{valueId}", token);
        return model.Map<TViewModel>(mapper);
    }

    public async Task SendPostMessage<TMessage>(string? url, TMessage message, CancellationToken token)
        where TMessage : BaseDto
    {
        var client = await GetClientAsync<TMessage>();
        
        var response = await client.PostAsJsonAsync(GetFullUrl<TMessage>(url), message, token);

        if (!response.IsSuccessStatusCode)
        {
            Logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }

    public async Task SendPutMessage<TMessage>(string? url, TMessage message, CancellationToken token)
        where TMessage : BaseDto
    {
        var client = await GetClientAsync<TMessage>();
        var response = await client.PutAsJsonAsync(GetFullUrl<TMessage>(url), message, token);

        if (!response.IsSuccessStatusCode)
        {
            Logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }

    public async Task SendDeleteMessage<TMessage>(string? url, CancellationToken token) where TMessage : BaseDto
    {
        var client = await GetClientAsync<TMessage>();
        var response = await client.DeleteAsync(GetFullUrl<TMessage>(url), token);

        if (!response.IsSuccessStatusCode)
        {
            Logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }

    protected override void HttpClientInit<TOut>()
    {
        if (HttpClient != null) return;
        HttpClient = CreateClient(ServiceName.ApiServer.ToString());
    }
    
    public override async ValueTask<HttpClient> GetClientAsync<T>()
    {
        HttpClientInit<T>();
        if (!Authorizate) return HttpClient!;
        
        var token = await tokenAuthenticationStateProvider.GetTokenAsync();
        logger.LogInformation($"Token: {token}");
        HttpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return HttpClient!;
    }
}
