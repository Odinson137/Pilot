using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Services;

namespace Pilot.BlazorClient.Service;

public class GateWayApiService(ILogger<BaseHttpService> logger, IHttpClientFactory httpClientFactory, IMapper mapper)
    : BaseHttpService(logger, httpClientFactory), IGateWayApiService
{
    public async Task<ICollection<TViewModel>> SendGetMessages<TOut, TViewModel>(
        string? url = null,
        BaseFilter? filter = null,
        CancellationToken token = default,
        params (string, string)[] queryParams)
        where TOut : BaseDto where TViewModel : BaseViewModel
    {
        var models = await SendGetMessages<TOut>(url, filter, token, queryParams);
        return models.MapList<TViewModel>(mapper);
    }

    public async Task<TViewModel> SendGetMessage<TOut, TViewModel>(
        int valueId,
        CancellationToken token = default, params (string, string)[] queryParams)
        where TOut : BaseDto where TViewModel : BaseViewModel
    {
        var models = await SendGetMessage<TOut>($"{valueId}", token, queryParams);
        return models.Map<TViewModel>(mapper);
    }

    public async Task SendPostMessage<TMessage>(string? url, TMessage message, CancellationToken token)
        where TMessage : BaseDto
    {
        HttpClientInit<TMessage>();

        var response = await GetClient<TMessage>().PostAsJsonAsync(GetFullUrl<TMessage>(url), message, token);

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
        HttpClientInit<TMessage>();

        var response = await GetClient<TMessage>().PostAsJsonAsync(GetFullUrl<TMessage>(url), message, token);

        if (!response.IsSuccessStatusCode)
        {
            Logger.LogInformation("Ошибка при отправке данных на сервер");
            var error = await response.Content.ReadAsStringAsync(token);
            throw new Exception(error);
        }
    }

    public async Task SendDeleteMessage<TMessage>(string? url, CancellationToken token) where TMessage : BaseDto
    {
        HttpClientInit<TMessage>();

        var response = await GetClient<TMessage>().DeleteAsync(GetFullUrl<TMessage>(url), token);

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
}
